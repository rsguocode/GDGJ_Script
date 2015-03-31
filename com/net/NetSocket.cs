using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using com.game;
using com.game.module.SystemData;
using com.net.debug;
using com.net.interfaces;
using com.net.p8583;
using com.u3d.bases.debug;
using Proto;

namespace com.net
{
    internal class NetSocket : ISocket
    {
        private readonly LinkedList<byte[]> dataList = new LinkedList<byte[]>();
        private readonly byte[] headBuffer;
        private readonly LinkedList<String> netStatusList = new LinkedList<String>();
        private byte[] bodyBuffer;
        private bool isReceive = true;
        private NetMsgCallback msgCallback;
        private int msgLen;

        private string netStatus = NetParams.LINK_FAIL;
        private NetStatusCallback statusCallback;
        private TcpClient tcpClient;

        public NetSocket()
        {
            headBuffer = new byte[2];
            tryNum = 1;
            tryableNum = 1;
        }

        public int tryNum { get; set; } //连接重连次数

        public int tryableNum { get; set; } //最大重连次数
        public int port { get; set; }
        public string ip { get; set; }


        //连接
        public void connect(string ip, int port)
        {
            //连接前先关闭
            if (tcpClient != null && tcpClient.Client != null)
            {
                tcpClient.Client.Close();
                tcpClient.Close();
                tcpClient = null;
            }

            if (!connected())
            {
                try
                {
                    setNetStatus(NetParams.LINKING);
                    this.ip = ip;
                    this.port = port;
                    IPAddress address = IPAddress.Parse(ip);
                    var iPEndPoint = new IPEndPoint(address, port);
                    Log.info(this, "-connect() " + "第" + tryNum + "次连接服务器 [" + iPEndPoint + "] [Start]");
                    tcpClient = new TcpClient();
                    tcpClient.NoDelay = true;
                    tcpClient.SendTimeout = 3000; //3秒
                    tcpClient.ReceiveTimeout = 3000; //3秒
                    tcpClient.BeginConnect(address, port, ConnectCallBack, this);
                }
                catch (Exception ex)
                {
                    NetLog.error(this, "-connect() 连接服务器异常：" + ex);
                    //失败重连
                    retryConnect();
                }
            }
        }

        //连接回调


        //重连
        public void retryConnect()
        {
            isReceive = false;
            if (tryNum < tryableNum)
            {
                tryNum++;
                connect(ip, port);
            }
            else
            {
                NetLog.error(this, "-retryConnect() 请求网络连接失败");
                close();
            }
        }

        //设置开始接收数据

        public void send(MemoryStream msdata, byte framID, byte wayID)
        {
            //NetLog.info(this, "发送协议数据：" + framID + " " + wayID);
            send(NetData.Encode(msdata, framID, wayID));
        }

        /*public void send(byte framID, byte wayID)
        {
            byte[] data = new byte[4] { 0, 2, framID, wayID };
            send(data);
            
        }*/

        //帧更新执行
        public void Update()
        {
            lock (netStatusList)
            {
                //检查连接状态变动状态
                if (netStatusList.Count > 0)
                {
                    if (statusCallback != null)
                    {
                        //状态通知
                        statusCallback(netStatusList.First.Value);
                        netStatusList.RemoveFirst();
                    }
                }
            }

            if (connected())
            {
                try
                {
                    //有数据要接收
                    while (tcpClient.Client.Available >= 2) //仍然有数据可以处理
                    {
                        //NetLog.warin(this, "收到数据");
                        if (msgLen == 0)
                        {
                            tcpClient.Client.Receive(headBuffer);
                            msgLen = ByteUtil.Byte2Int(headBuffer);
                            //NetLog.warin(this, "receive data head length: " + this.msgLen);
                            bodyBuffer = new byte[msgLen];
                        }
                        else
                        {
                            if (tcpClient.Client.Available >= msgLen) //数据完整
                            {
                                tcpClient.Client.Receive(bodyBuffer);
                                //NetLog.warin(this, "Receive bodyBuffer success");
                                var netData = new NetData(bodyBuffer);
                                AppNet.main.receiveNetMsg(netData);
                                bodyBuffer = null;
                                msgLen = 0;
                            }
                            else
                            {
                                break; //数据不完整-退出 下次update再处理
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    NetLog.error(this, "-Update() Socket接收数据出错," + ex);
                    close();
                }
            }
            else
            {
                if (netStatus.Equals(NetParams.LINK_OK))
                {
                    close();
                }
            }
        }

        //是否连接
        public bool connected()
        {
            return tcpClient != null && tcpClient.Client != null && tcpClient.Client.Connected &&
                   netStatus.Equals(NetParams.LINK_OK);
        }

        //关闭连接
        public void close()
        {
            stopReceive();
            if (tcpClient != null && tcpClient.Client != null)
            {
                tcpClient.Client.Close();
                tcpClient.Close();
                tcpClient = null;
            }
            NetLog.info(this, "-close() 已关闭与服务器连接！");
            setNetStatus(NetParams.LINK_FAIL);

            bodyBuffer = null;
            msgLen = 0;
        }

        //状态监听
        public void statusListener(NetStatusCallback listener)
        {
            statusCallback = listener;
        }

        //消息监听
        public void msgListenner(NetMsgCallback listener)
        {
            msgCallback = listener;
        }


        public void setTryableNum(int num)
        {
            tryableNum = num;
        }

        public int getTryableNum()
        {
            return tryableNum;
        }

        private void ConnectCallBack(IAsyncResult asyncresult)
        {
            var net = asyncresult.AsyncState as NetSocket;
            try
            {
                if (net.tcpClient.Client != null && net.tcpClient.Client.Connected)
                {
                    //连接成功
                    net.tcpClient.EndConnect(asyncresult);

                    NetLog.info(net, "-ConnectCallBack()" + "第" + net.tryNum + "次连接服务器 [" + net.ip + "] [OK]");
                    net.startReceive();
                    net.tryNum = 0;
                    net.msgLen = 0;

                    //重连时，先将缓存的数据清空
                    lock (dataList)
                    {
                        dataList.Clear();
                    }
                    net.setNetStatus(NetParams.LINK_OK);
                    //首先发送
                    var netdata = new MemoryStream();
                    send(netdata, 1, 0);
                }
                else
                {
                    NetLog.info(net, "-ConnectCallBack()" + "第" + net.tryNum + "次连接服务器 [" + net.ip + "] [Fail]");
                    net.retryConnect();
                }
            }
            catch (Exception ex)
            {
                NetLog.error(net, "-ConnectCallBack() 连接服务器异常：" + ex);

                net.retryConnect();
            }
        }

        private void SendCallBack(IAsyncResult asyncresult)
        {
            var data = asyncresult.AsyncState as byte[];
            try
            {
                int sendNum = tcpClient.Client.EndSend(asyncresult);
                sendNext();
            }
            catch (Exception ex)
            {
                NetLog.error(this, "-SendCallBack() 发送数据失败" + ex.Message);
                close();
            }
        }

        private void startReceive()
        {
            isReceive = true;
        }

        //停止接收数据
        private void stopReceive()
        {
            isReceive = false;
        }

        private void send(byte[] data)
        {
            if (connected())
            {
                /*lock (dataList)
                {
                    //添加数据到发送队列
                    dataList.AddLast(data);
                    if (dataList.Count == 1)
                    {
                        //当前即为第一个数据则发送
                        sendAnsy(data);
                    }
                }*/
                try
                {
                    Log.info(this, "-Send 成功发送数据 " + data.Length + "bytes: " +"  ServerTimestamp:"+ ServerTime.Instance.Timestamp);
                    tcpClient.Client.Send(data);
                }
                catch (Exception ex)
                {
                    NetLog.error(this, "-send() 发送数据出错," + ex);
                    close();
                }
            }
            else
            {
                NetLog.error(this, "-send()  网络未连接,关闭网络请重新连接");
                close();
            }
        }

        //异步发送数据
        private void sendAnsy(byte[] data)
        {
            if (connected())
            {
                try
                {
                    tcpClient.Client.BeginSend(data, 0, data.Length, SocketFlags.None, SendCallBack, data);
                }
                catch (Exception ex)
                {
                    NetLog.error(this, "-sendAnsy() 发送数据出错," + ex);
                    close();
                }
            }
            else
            {
                NetLog.error(this, "-sendAnsy()  网络未连接,关闭网络请重新连接");
                close();
            }
        }

        private void sendNext()
        {
            lock (dataList)
            {
                //移除已经发送的数据
                dataList.RemoveFirst();
                if (dataList.Count > 0)
                {
                    //仍有数据则继续发送
                    sendAnsy(dataList.First.Value);
                }
            }
        }

        //设置状态
        private void setNetStatus(string netStatus)
        {
            if (this.netStatus == null || !this.netStatus.Equals(netStatus))
            {
                this.netStatus = netStatus;
                lock (netStatusList)
                {
                    if (statusCallback != null)
                    {
                        //状态变动则加入状态列表以供通知
                        netStatusList.AddLast(netStatus);
                    }
                }
            }
        }
    }
}
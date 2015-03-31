using System.Globalization;
using com.game.data;
using com.game.manager;
using com.game.sound;

namespace com.game.Public.Message
{
    public class ErrorCodeManager
    {
        public static void ShowError(ushort errorCode)
        {
            SysErrorCodeVo errorCodeVo = BaseDataMgr.instance.GetErrorCodeVo(uint.Parse(errorCode.ToString(CultureInfo.InvariantCulture)));
            string result = "";
            if (errorCodeVo == null)
            {
                result = string.Format("Errorcode {0} is not defined", errorCode);
            }
            else
            {
                result = errorCodeVo.desc;
            }
            MessageManager.Show(result);

			SoundMgr.Instance.PlayUIAudio(SoundId.Sound_ConfirmClose);
        }
    }
}

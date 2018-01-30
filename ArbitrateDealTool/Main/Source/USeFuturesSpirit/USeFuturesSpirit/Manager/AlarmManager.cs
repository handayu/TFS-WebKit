using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Media;
using System.Runtime.InteropServices;

namespace USeFuturesSpirit
{
    public class AlarmManager
    {
        private bool m_warningFlag = false;

        private SoundPlayer m_player = new SoundPlayer();
        private string soundLocation2 = @"sound\连接中断.wav";
        public void FireAlarm(AlarmType alarmType)
        {
            FireAlarm(alarmType, false);
        }

        public void FireAlarm(AlarmType alarmType,bool loop)
        {
            try
            {
                string soundLocation = @"sound\成交通知.wav";
                string soundLocation2 = @"sound\连接中断.wav";
                SoundPlayer player = new SoundPlayer();
                player.SoundLocation = soundLocation;
                player.Load();
                player.PlayLooping();
                string s = "";

                //SoundPlayer player2 = new SoundPlayer();

                //player2.SoundLocation = soundLocation;
                //player2.Load();
                //player2.PlayLooping();

                //PlaySound(soundLocation, true, true, false);




            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }
        }

        public void StopWarning()
        {

        }


       
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meetings
{
  internal class Meeting
  {
    public string Title { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int ReminderTime { get; set; }

    public Meeting(string title, DateTime startTime, DateTime endTime, int reminderTime)
    {
      Title = title;
      StartTime = startTime;
      EndTime = endTime;
      ReminderTime = reminderTime; ;
    }
  }
}

namespace Meetings
{
  internal class Program
  {
    static List<Meeting> meetings = new List<Meeting>();
    static void Main(string[] args)
    {

      meetings.Add(new Meeting("Встреча 1", DateTime.Parse("2023-10-10 14:00"), DateTime.Parse("2023-10-10 15:00"), 15));
      meetings.Add(new Meeting("Встреча 2", DateTime.Parse("2023-10-11 10:30"), DateTime.Parse("2023-10-11 11:30"), 30));
      meetings.Add(new Meeting("Встреча 3", DateTime.Parse("2023-10-10 14:00"), DateTime.Parse("2023-10-10 15:00"), 15));
      meetings.Add(new Meeting("Встреча 4", DateTime.Parse("2023-10-11 10:30"), DateTime.Parse("2023-10-11 11:30"), 30));


      foreach (var meeting in meetings)
      {
        SetMeetingReminder(meeting);
      }

      while (true)
      {
        Console.WriteLine("Меню:");
        Console.WriteLine("1. Добваить встречу");
        Console.WriteLine("2. Изменить встречу");
        Console.WriteLine("3. Удалить встречу");
        Console.WriteLine("4. Просмотр встреч");
        Console.WriteLine("5. Экспорт в текстовый файл");

        int Choice = int.Parse(Console.ReadLine());

        switch (Choice)
        {
          case 1:
            AddMeeting();
            break;
          case 2:
            EditMeeting();
            break;
          case 3:
            DeleteMeeting();
            break;
          case 4:
            ShowMeetings();
            break;
          case 5:
            ExportToTxt();
            break;
          default:
            Console.WriteLine("Некорректынй выбор. Попробуйте еще раз");
            break;
        }
      }
    }
    #region "Методы"
    /// <summary>
    /// Добавление встречи
    /// </summary>
    static void AddMeeting()
    {
      Console.WriteLine("Введите название встречи:");
      string title = Console.ReadLine();

      DateTime startTime;
      while (true)
      {
        Console.WriteLine("Укажите время начала встречи (формат чч:мм dd.MM.yyyy):");
        string startTimeStr = Console.ReadLine();
        try
        {
          startTime = DateTime.ParseExact(startTimeStr, "HH:mm dd.MM.yyyy", null);
          if (startTime <= DateTime.Now)
          {
            Console.WriteLine("Ошибка: Нельзя создать встречу с прошедшей датой или текущей датой.");
          }
          else
          {
            break; 
          }
        }
        catch (FormatException)
        {
          Console.WriteLine("Ошибка: Некорректный формат даты и времени. Используйте формат 'чч:мм dd.MM.yyyy'.");
        }
      }

      DateTime endTime;
      while (true)
      {
        Console.WriteLine("Укажите время конца встречи (по умолчанию 1 час):");
        string endTimeStr = Console.ReadLine();
        try
        {
          if (string.IsNullOrEmpty(endTimeStr))
          {
            endTime = startTime.AddHours(1);
          }
          else
          {
            endTime = DateTime.ParseExact(endTimeStr, "HH:mm dd.MM.yyyy", null);
          }
          break; 
        }
        catch (FormatException)
        {
          Console.WriteLine("Ошибка: Некорректный формат даты и времени. Используйте формат 'чч:мм dd.MM.yyyy'.");
        }
      }

      int reminderTime;
      while (true)
      {
        Console.WriteLine("Введите время напоминания (в минутах до начала встречи):");
        string reminderTimeStr = Console.ReadLine();
        try
        {
          reminderTime = int.Parse(reminderTimeStr);
          break;
        }
        catch (FormatException)
        {
          Console.WriteLine("Ошибка: Введите число.");
        }
      }

      Meeting newMeeting = new Meeting(title, startTime, endTime, reminderTime);
      meetings.Add(newMeeting);
      Console.WriteLine("Встреча добавлена.");

      SetMeetingReminder(newMeeting);
    }
    /// <summary>
    /// Редактирование встречи
    /// </summary>
    static void EditMeeting()


    {
      Console.WriteLine("Введите название встречи, которую хотите изменить:");
      string titleToEdit = Console.ReadLine();

      Meeting meetingToEdit = meetings.FirstOrDefault(m => m.Title == titleToEdit);

      if (meetingToEdit == null)
      {
        Console.WriteLine("Встреча не найдена.");
        return;
      }

      Console.WriteLine("Выбранная встреча: " + meetingToEdit.Title);

      Console.WriteLine("Введите новое название встречи (или оставьте пустым для сохранения текущего):");
      string newTitle = Console.ReadLine();
      if (!string.IsNullOrEmpty(newTitle))
      {
        meetingToEdit.Title = newTitle;
      }

      Console.WriteLine("Введите новую дату и время начала встречи (или оставьте пустым для сохранения текущего):");
      string newStartDateTimeStr;
      try
      {
        newStartDateTimeStr = Console.ReadLine();
        if (!string.IsNullOrEmpty(newStartDateTimeStr))
        {
          DateTime newStartDateTime = DateTime.ParseExact(newStartDateTimeStr, "HH:mm dd.MM.yyyy", null);
          meetingToEdit.StartTime = newStartDateTime;
        }
      }
      catch (FormatException)
      {
        Console.WriteLine("Ошибка: Некорректный формат даты и времени. Используйте формат 'чч:мм dd.MM.yyyy'.");
        return;
      }

      Console.WriteLine("Введите новое время конца встречи (или оставьте пустым для сохранения текущей):");
      string newEndTimeStr;
      try
      {
        newEndTimeStr = Console.ReadLine();
        if (!string.IsNullOrEmpty(newEndTimeStr))
        {
          DateTime newEndTime = DateTime.ParseExact(newEndTimeStr, "HH:mm dd.MM.yyyy", null);
          meetingToEdit.EndTime = newEndTime;
        }
      }
      catch
      {
        Console.WriteLine("Ошибка: Некорректный формат даты и времени. Используйте формат 'чч:мм dd.MM.yyyy'.");
        return;
      }

      Console.WriteLine("Введите новое время напоминания в минутах (или оставьте пустым для сохранения текущего):");
      string newReminderStr;
      try
      {
        newReminderStr = Console.ReadLine();
        if (!string.IsNullOrEmpty(newReminderStr))
        {
          int newReminderTime = int.Parse(newReminderStr);
          meetingToEdit.ReminderTime = newReminderTime;
          Console.WriteLine("Встреча изменена.");
        }
      }
      catch (FormatException)
      {
        Console.WriteLine("Ошибка: Введите число.");
      }
      SetMeetingReminder(meetingToEdit);
    }
    /// <summary>
    /// Удаляет встречу
    /// </summary>
    static void DeleteMeeting()
    {
      Console.WriteLine("Введите название встречи, которую хотите удалить:");
      string titleToDelete = Console.ReadLine();
      Meeting meetingToDelete = meetings.FirstOrDefault(m => m.Title == titleToDelete);

      if (meetingToDelete == null)
      {
        Console.WriteLine("Встреча не найдена.");
        return;
      }

      meetings.Remove(meetingToDelete);
      Console.WriteLine("Встреча удалена.");
    }
    /// <summary>
    /// Вывод список встреч на выбранный день
    /// </summary>
    static void ShowMeetings()
    {
      Console.WriteLine("Введите дату для просмотра расписания:");
      DateTime dateToView; 
      try
      {
        dateToView = DateTime.ParseExact(Console.ReadLine(), "dd.MM.yyyy", null);
      }
      catch (FormatException)
      {
        Console.WriteLine("Ошибка: Некорректный формат даты и времени. Используйте формат 'чч:мм dd.MM.yyyy'.");
        return;
      }
      Console.WriteLine($"Расписание на {dateToView.ToShortDateString()}:");
      foreach (var meeting in meetings)
      {
        if (meeting.StartTime.Date == dateToView.Date)
        {
          Console.WriteLine($"{meeting.StartTime.TimeOfDay} - {meeting.Title}");
        }
      }
    }

    /// <summary>
    /// Экспорт в txt файл
    /// </summary>
    static void ExportToTxt()
    {
      Console.WriteLine("Введите дату для экспорта расписания в файл:");
      DateTime dateToExport = DateTime.ParseExact(Console.ReadLine(), "dd.MM.yyyy", null);

      string fileName = $"Schedule_{dateToExport.ToShortDateString()}.txt";

      using (StreamWriter writer = new StreamWriter(fileName))
      {
        writer.WriteLine($"Расписание на {dateToExport.ToShortDateString()}:");
        foreach (var meeting in meetings)
        {
          if (meeting.StartTime.Date == dateToExport.Date)
          {
            writer.WriteLine($"{meeting.StartTime.TimeOfDay} - {meeting.Title}");
          }
        }
      }

      Console.WriteLine($"Расписание экспортировано в файл {fileName}.");
    }
    /// <summary>
    /// Напоминалка
    /// </summary>
    static void SetMeetingReminder(Meeting meeting)
    {
      DateTime reminderTime = meeting.StartTime.AddMinutes(-meeting.ReminderTime);

      TimeSpan timeToReminder = reminderTime - DateTime.Now;

      if (timeToReminder.TotalMinutes > 0)
      {
        Timer timer = new Timer(ShowMeetingReminder, meeting, (int)timeToReminder.TotalMilliseconds, Timeout.Infinite);
      }
    }
    /// <summary>
    /// Отображение напоминания о встрече
    /// </summary>
    static void ShowMeetingReminder(object meetingObj)
    {
      Meeting meeting = (Meeting)meetingObj;
      Console.WriteLine($"Напоминание: Встреча '{meeting.Title}' начнется через {meeting.ReminderTime} минут.");
    }
    #endregion
  }
}
# 🕒 TaskSchedulerQuartz

## Overview
**TaskSchedulerQuartz** is a .NET 8 Web API project that demonstrates how to schedule and manage recurring or one-time background tasks using the **Quartz.NET** library.

This project provides a complete example of how to use Quartz for:
- Running **daily, weekly, and monthly jobs** automatically.
- Scheduling **specific one-time jobs** for future dates.
- Handling **time-zone–aware tasks** (e.g., IST, UTC).
- Running **repeating interval jobs**.
- Managing all job schedules from a **single configuration file**.

---

## ⚙️ What is Quartz.NET?
**Quartz.NET** is a powerful, open-source job scheduling library for .NET.  
It allows developers to run background jobs at specific times, intervals, or according to complex schedules using **cron expressions**.

### ✅ Key Features
- Cron-like scheduling (e.g., “run every Friday at 3 PM”).
- Multiple jobs can run concurrently.
- Fully supports **Dependency Injection (DI)**.
- Timezone-aware scheduling using `.InTimeZone()`.
- Persistent job stores (for databases) supported in production.

---

## 🧩 Project Structure

```
TaskSchedulerQuartz/
│
├── Jobs/
│   ├── EmailJob.cs
│   ├── ReportJob.cs
│   ├── BackupJob.cs
│   ├── SpecialJob.cs
│   ├── LastDayOfMonthJob.cs
│   └── IntervalJob.cs
│
├── Scheduler/
│   └── QuartzJobScheduler.cs
│
├── Program.cs
└── TaskSchedulerQuartz.csproj
```

---

## 🧠 How Quartz Scheduling Works

Quartz jobs are defined as classes implementing `IJob`.  
Each job’s logic runs inside the `Execute()` method.

Triggers define **when** a job runs.  
They can use **cron expressions** or **simple intervals**.

🕒 Full Quartz Cron Expression Structure:
Quartz uses a 6 or 7-field cron format — the 7th field (year) is optional.
```
┌───────────── second (0–59)
│ ┌─────────── minute (0–59)
│ │ ┌───────── hour (0–23)
│ │ │ ┌─────── day of month (1–31)
│ │ │ │ ┌───── month (1–12 or JAN–DEC)
│ │ │ │ │ ┌─── day of week (1–7 or SUN–SAT)
│ │ │ │ │ │ ┌ year (optional)
│ │ │ │ │ │ │
│ │ │ │ │ │ │
* * * * * ? *
```

The **optional 7th field** represents the year.

---

## 🧩 Field Explanation

| **Field** | **Allowed Values** | **Special Characters** | **Description** |
|:--|:--|:--|:--|
| **Seconds** | 0–59 | `, - * /` | Seconds to fire on |
| **Minutes** | 0–59 | `, - * /` | Minutes to fire on |
| **Hours** | 0–23 | `, - * /` | Hours to fire on |
| **Day of Month** | 1–31 | `, - * ? / L W C` | Day of the month |
| **Month** | 1–12 or JAN–DEC | `, - * /` | Month |
| **Day of Week** | 1–7 or SUN–SAT | `, - * ? / L C #` | Day of the week |
| **Year (optional)** | 1970–2099 | `, - * /` | Optional year field |

---

## 🔹 Common Special Characters

| **Character** | **Meaning** | **Example** |
|:--|:--|:--|
| `*` | Any value (every unit) | `* * * * * ?` → every second |
| `?` | No specific value (used to avoid conflict between day-of-month and day-of-week) | `0 0 12 ? * MON-FRI` → every weekday at noon |
| `/` | Increment (every N units) | `0/10 * * * * ?` → every 10 seconds |
| `,` | Multiple values | `0 0 10,14,16 * * ?` → 10 AM, 2 PM, 4 PM |
| `-` | Range | `0 15 10-12 * * ?` → every 15th minute during 10–12 AM |
| `L` | Last day (of month or week) | `0 0 15 L * ?` → 3 PM on last day of month |
| `W` | Nearest weekday | `0 0 9 15W * ?` → 9 AM on the nearest weekday to 15th of month |
| `#` | Nth weekday of the month | `0 0 9 ? * 5#3` → 9 AM on the 3rd Friday of every month |
| `C` | Calendar-based — used with `L` or alone to refer to the **nearest day/week to the calendar start** (depends on configuration) | `5C` → 5 days after the calendar’s start date; `1C` in day-of-week means first weekday of the calendar month |

---

## 💡 Examples

| **Expression** | **Description** |
|:--|:--|
| `0 0 15 * * ?` | Every day at 3:00 PM |
| `0 0 15 ? * FRI` | Every Friday at 3:00 PM |
| `0 0 15 30 * ?` | Every 30th day of the month at 3:00 PM |
| `0 0 15 L * ?` | On the last day of each month at 3:00 PM |
| `0 0 15 ? * FRI 2025` | Every Friday at 3:00 PM in the year 2025 only |

---

✅ **Notes:**
- You can include the **7th "Year" field** (optional) in Quartz cron expressions.
- Use **`L`** to represent the **last day of the month** (or the last specific weekday).

---

## 🗓️ Configured Jobs (in `QuartzJobScheduler.cs`)

| Job | Type | Schedule | Description |
|------|------|-----------|-------------|
| **EmailJob** | Daily | `0 0 15 * * ?` | Runs daily at 3:00 PM |
| **BackupJob** | Weekly | `0 0 15 ? * FRI` | Runs every Friday at 3:00 PM |
| **ReportJob** | Monthly | `0 0 15 30 * ?` | Runs every 30th at 3:00 PM |
| **LastDayOfMonthJob** | Monthly | `0 0 15 L * ?` | Runs on last day of each month at 3:00 PM |
| **SpecialJob** | One-time | DateTime: `2025-10-03 15:15` | Runs once on a specific date/time |
| **IntervalJob** | Interval | Every 10 seconds | Repeats indefinitely |
| **IntervalJobIST** | Interval + Timezone | Every 10 seconds (IST-based start) | Timezone-aware repeat |

---

## 🌏 Time Zone Handling

Quartz runs using the **server’s local time zone** by default.  
If your server runs in UTC, you can convert time zones manually:

```csharp
var indiaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
var targetTime = TimeZoneInfo.ConvertTimeToUtc(
    new DateTime(2025, 10, 3, 15, 15, 0), indiaTimeZone);
```

You can also assign a time zone directly in a trigger:

```csharp
.WithCronSchedule("0 0 15 * * ?", x =>
    x.InTimeZone(TimeZoneInfo.FindSystemTimeZoneById("Asia/Kolkata")));
```

---

## 🧱 Code Example (From `QuartzJobScheduler.cs`)

```csharp
var emailJobKey = new JobKey("EmailJob");
q.AddJob<EmailJob>(opts => opts.WithIdentity(emailJobKey));
q.AddTrigger(opts => opts
    .ForJob(emailJobKey)
    .WithIdentity("EmailJob-trigger")
    .WithCronSchedule("0 0 15 * * ?"));
```

This means:
> Run **EmailJob** every day at **3:00 PM** server time.

---

## 🚀 Running the Project

1. Install required NuGet packages:

```bash
dotnet add package Quartz
dotnet add package Quartz.Extensions.Hosting
```

2. Register the scheduler in `Program.cs`:

```csharp
builder.Services.AddQuartz(q =>
{
    QuartzJobScheduler.ConfigureJobs(q);
});
builder.Services.AddQuartzHostedService(opt => opt.WaitForJobsToComplete = true);
```

3. Run the application:

```bash
dotnet run
```

You’ll see console logs when each scheduled job executes.

---

## 💡 Why Quartz Instead of BackgroundService?

| Feature | BackgroundService | Quartz.NET |
|----------|-------------------|-------------|
| Simple Repeating Jobs | ✅ Yes | ✅ Yes |
| Cron-like Scheduling | ❌ No | ✅ Yes |
| Multiple Jobs | ⚠️ Manual Handling | ✅ Built-in |
| Dependency Injection | ✅ Yes | ✅ Yes |
| Persistent Job Store | ❌ No | ✅ Yes |
| Timezone Support | ❌ Manual | ✅ Built-in |
| Misfire Handling | ❌ No | ✅ Yes |

**In short:** Quartz is a more robust, production-ready scheduler for enterprise apps.

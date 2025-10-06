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

A **cron expression** has the following format:

```
┌───────────── second (0–59)
│ ┌─────────── minute (0–59)
│ │ ┌───────── hour (0–23)
│ │ │ ┌─────── day of month (1–31)
│ │ │ │ ┌───── month (1–12)
│ │ │ │ │ ┌─── day of week (0–6 or SUN–SAT)
│ │ │ │ │ │
│ │ │ │ │ │
* * * * * ?
```

The **optional 7th field** represents the year.

| Symbol | Meaning | Example |
|---------|----------|----------|
| `*` | Every value | Every minute/hour/day |
| `?` | No specific value | Used when another field is specified |
| `/` | Increment | `0/10` means every 10 seconds |
| `L` | Last | `L` in day-of-month = last day of month |
| `,` | List | `MON,WED,FRI` |
| `-` | Range | `10-12` = between 10 and 12 |

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

---

## 🧾 License
This project is licensed under the MIT License.

---

## 👨‍💻 Author
**Rohit** — built to demonstrate robust task scheduling in .NET using Quartz.NET.

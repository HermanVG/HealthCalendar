# HealthCalendar

This is a very basic application. We were originally a group of five, but due to a lot of issues two members ended up leaving the group. The result was a lot of wasted time and them deleting all the work they had done. We had to almost start over so all we had time for was this basic application.

### How to run the application

This application is built with node.js v22.20.0. To it run the command: `dotnet run` from the HealthCalendar-folder

### More info

There are two types of Users, Patient and Worker, both with separate logins. Users cannot be registered, but the database is seeded with both Patients and Workers. Below you can find their login-info:

Login info for Patients:
Patient 1:
email: a@a.com, password: a
Patient 2:
email: c@a.com, password: c

Login info for Workers:
Worker 1:
email: a@a.com, password: a
Worker 2:
email: b@a.com, password: b

Patients can create, read, update and delete Events. Each Patient is assigned to a Worker.

Workers can read their assigned patients' events. Workers can also create, read and delete their availability.

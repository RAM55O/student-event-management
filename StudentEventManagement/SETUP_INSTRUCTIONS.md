# Event Management System - Setup Instructions

## Changes Made

I've successfully enhanced your Event Management System with the following features:

### 1. Enhanced Sign-Up Page
- Added detailed user registration fields:
  - First Name (required)
  - Surname (required)
  - Mobile Number (required)
  - Email (required)
  - Username (required)
  - Password (required)

### 2. Updated User Model
- Extended the User class with new properties:
  - FirstName
  - Surname
  - MobileNumber
  - Email

### 3. New Event Management Home Page
- Created `/EventHome` page that displays after successful login
- Shows user profile information
- Displays upcoming events
- Provides quick action buttons based on user role

### 4. Updated Authentication Flow
- Login now redirects to Event Management home page
- Main index page redirects authenticated users to Event Management home

## Database Update Required

**IMPORTANT**: You need to update your database schema to include the new user fields.

### Option 1: Using Entity Framework Migration (Recommended)

1. Stop the running application (Ctrl+C in terminal)
2. Run the following commands in the StudentEventManagement directory:

```bash
dotnet ef migrations add AddUserDetails
dotnet ef database update
```

### Option 2: Manual SQL Update

Execute the SQL script `update_users_table.sql` in your MySQL database:

```sql
ALTER TABLE Users 
ADD COLUMN FirstName VARCHAR(100),
ADD COLUMN Surname VARCHAR(100),
ADD COLUMN MobileNumber VARCHAR(20),
ADD COLUMN Email VARCHAR(255);
```

## How to Test

1. **Stop the current application** if it's running
2. **Update the database** using one of the methods above
3. **Build and run the application**:
   ```bash
   dotnet build
   dotnet run
   ```
4. **Test the new sign-up flow**:
   - Navigate to `/Account/SignUp`
   - Fill in all the required fields
   - Submit the form
5. **Test the login flow**:
   - Use the credentials you just created
   - Verify you're redirected to the Event Management home page
   - Check that your profile information is displayed correctly

## Features

### Sign-Up Page Features:
- ✅ First Name field
- ✅ Surname field  
- ✅ Mobile Number field
- ✅ Email field
- ✅ Username field
- ✅ Password field
- ✅ Form validation
- ✅ Bootstrap styling

### Authentication Features:
- ✅ Database ID and password matching
- ✅ Successful login redirects to Event Management home
- ✅ Session management
- ✅ User profile display

### Event Management Home Features:
- ✅ User profile card with full details
- ✅ Quick action buttons
- ✅ Upcoming events display
- ✅ Role-based functionality (Admin vs User)
- ✅ Responsive design

## File Changes Made:

1. `Data/User.cs` - Added new user properties
2. `Pages/Account/SignUp.cshtml` - Enhanced form with new fields
3. `Pages/Account/SignUp.cshtml.cs` - Updated to handle new fields
4. `Pages/Account/Login.cshtml.cs` - Updated redirect destination
5. `Pages/EventHome.cshtml` - New Event Management home page
6. `Pages/EventHome.cshtml.cs` - Code-behind for Event Management home
7. `Pages/Index.cshtml.cs` - Updated to redirect authenticated users
8. `update_users_table.sql` - Database migration script

The system now provides a complete user registration and authentication flow with detailed user information and a dedicated Event Management interface!
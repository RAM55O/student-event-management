# 🚀 Quick Start Deployment

## Choose Your Method:

### 🎯 **EASIEST: Railway.app** (5 minutes)

1. **Sign up**: https://railway.app
2. **Push code to GitHub** (see below)
3. **Click "New Project" → "Deploy from GitHub"**
4. **Add MySQL database** in Railway
5. **Done!** Auto-deployed at: `https://your-app.up.railway.app`

---

### 💪 **BEST FOR .NET: Azure** (10 minutes)

1. **Get Azure Student Account**: https://azure.microsoft.com/free/students/
   - $100 credit, no credit card needed

2. **Install Azure CLI**:
   ```powershell
   winget install Microsoft.AzureCLI
   ```

3. **Run these commands** (one by one):

   ```powershell
   # Login
   az login

   # Deploy your app (this does EVERYTHING automatically!)
   cd C:\xampp\htdocs\b\StudentEventManagement
   az webapp up --name student-event-$(Get-Random -Maximum 9999) --runtime "DOTNET:9.0" --sku F1 --location eastus
   ```

4. **Set up database**:
   - Option 1: Use free MySQL at https://www.freemysqlhosting.net/
   - Option 2: Use Azure MySQL (uses your $100 credit)

5. **Update connection string** in Azure Portal → Configuration → Connection strings

6. **Done!** Your app is live at: `https://student-event-XXXX.azurewebsites.net`

---

## 📤 Push to GitHub First

Run these commands in PowerShell:

```powershell
# Navigate to your project
cd C:\xampp\htdocs\b

# Initialize git
git init
git add .
git commit -m "Initial commit - Student Event Management"

# Create a new repo on GitHub.com (name it: student-event-management)
# Then run:
git remote add origin https://github.com/YOUR_USERNAME/student-event-management.git
git branch -M main
git push -u origin main
```

---

## ⚡ **Super Quick Railway Deployment**

After pushing to GitHub:

1. Go to https://railway.app
2. Sign in with GitHub
3. Click "New Project"
4. Select "Deploy from GitHub repo"
5. Choose "student-event-management"
6. Click "+ New" → "Database" → "MySQL"
7. Copy MySQL connection string
8. Add environment variable:
   - Key: `ConnectionStrings__DefaultConnection`
   - Value: (paste Railway MySQL URL)
9. **Auto-deploys!** ✅

---

## 🗄️ Free Database Options

- **Railway MySQL**: Included (easiest)
- **PlanetScale**: https://planetscale.com (5GB free)
- **FreeMySQLHosting**: https://www.freemysqlhosting.net (5MB free)
- **Azure MySQL**: Use your $100 student credit

---

## 🐛 Common Issues

### "Connection string not found"
→ Add connection string in hosting platform's environment variables

### "404 Not Found"
→ Make sure you deployed from the correct directory

### "Database doesn't exist"
→ Run migrations: `dotnet ef database update` (after connecting to production DB)

---

## ✅ Files Created for You

- ✅ `.gitignore` - Excludes unnecessary files from Git
- ✅ `Dockerfile` - For Railway/Render deployment
- ✅ `web.config` - For Azure deployment
- ✅ `Program.cs` - Updated for cloud hosting
- ✅ GitHub Actions workflow (optional auto-deploy)

---

## 🎯 Next: Choose ONE Method

**For beginners**: Use **Railway** (easiest)  
**For best performance**: Use **Azure** (free with student account)  
**For Docker lovers**: Use **Render.com**

Ready to deploy? Let me know which method you choose! 🚀

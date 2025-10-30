# 🚀 Complete Deployment Guide - Student Event Management

## 📋 Prerequisites

### 1. Get Student Benefits (FREE)
- [ ] Sign up for **GitHub Student Developer Pack**: https://education.github.com/pack
- [ ] Sign up for **Azure for Students**: https://azure.microsoft.com/free/students/
  - Get $100 credit (NO credit card needed)
  - Requires: Student email + Student ID

---

## 🎯 DEPLOYMENT OPTIONS

Choose ONE of these methods:

---

## ✅ **METHOD 1: Azure App Service (RECOMMENDED)**

### **Step 1: Prepare Your Project**

1. **Install Azure CLI** (if not installed):
   ```powershell
   winget install Microsoft.AzureCLI
   ```
   Or download from: https://aka.ms/installazurecliwindows

2. **Login to Azure**:
   ```powershell
   az login
   ```
   This will open browser - login with your student account

### **Step 2: Create Azure Resources**

3. **Set your variables**:
   ```powershell
   $resourceGroup = "student-event-rg"
   $appName = "student-event-app-$(Get-Random -Maximum 9999)"
   $location = "eastus"
   ```

4. **Create Resource Group**:
   ```powershell
   az group create --name $resourceGroup --location $location
   ```

5. **Create App Service Plan (FREE tier)**:
   ```powershell
   az appservice plan create --name student-event-plan --resource-group $resourceGroup --sku F1 --is-linux
   ```

6. **Create Web App**:
   ```powershell
   az webapp create --resource-group $resourceGroup --plan student-event-plan --name $appName --runtime "DOTNET:9.0"
   ```

### **Step 3: Set Up Database**

**Option A: Use Free MySQL Provider**
- Sign up at: https://www.freemysqlhosting.net/
- Get connection string
- Update in Azure portal

**Option B: Use Azure MySQL (Uses your $100 credit)**
```powershell
# Create MySQL server (Basic tier - uses credit)
az mysql flexible-server create --resource-group $resourceGroup --name student-event-db --admin-user adminuser --admin-password YourStrongPassword123! --sku-name Standard_B1ms --tier Burstable --public-access 0.0.0.0
```

### **Step 4: Configure Connection String**

```powershell
# Set connection string in Azure
az webapp config connection-string set --resource-group $resourceGroup --name $appName --connection-string-type MySql --settings DefaultConnection="server=YOUR_DB_SERVER;port=3306;database=student_event_management;user=YOUR_USER;password=YOUR_PASSWORD"
```

### **Step 5: Deploy Your Application**

**Option A: Deploy via ZIP**
```powershell
# From project directory
cd C:\xampp\htdocs\b\StudentEventManagement
dotnet publish -c Release -o ./publish
Compress-Archive -Path ./publish/* -DestinationPath deploy.zip -Force
az webapp deployment source config-zip --resource-group $resourceGroup --name $appName --src deploy.zip
```

**Option B: Deploy via Git**
```powershell
# Enable local Git deployment
az webapp deployment source config-local-git --name $appName --resource-group $resourceGroup

# Get deployment URL
$gitUrl = az webapp deployment list-publishing-credentials --name $appName --resource-group $resourceGroup --query scmUri -o tsv

# Add remote and push
git init
git add .
git commit -m "Initial deployment"
git remote add azure $gitUrl
git push azure main
```

### **Step 6: Run Database Migrations**

```powershell
# SSH into your app
az webapp ssh --resource-group $resourceGroup --name $appName

# Or run migration locally pointing to Azure DB
# Update appsettings.json with Azure MySQL connection string
dotnet ef database update
```

### **Step 7: Access Your Live App**

```powershell
# Open in browser
az webapp browse --resource-group $resourceGroup --name $appName
```

Your app will be live at: `https://$appName.azurewebsites.net`

---

## ✅ **METHOD 2: Railway.app (Easier, Auto-Deploy)**

### **Step 1: Prepare Project**

1. Create account at: https://railway.app/ (Free $5/month credit)
2. Connect your GitHub account

### **Step 2: Push to GitHub**

```powershell
# Initialize git
git init
git add .
git commit -m "Initial commit"

# Create GitHub repo (go to github.com, create new repo)
# Then push
git remote add origin https://github.com/YOUR_USERNAME/student-event-management.git
git branch -M main
git push -u origin main
```

### **Step 3: Deploy on Railway**

1. Go to Railway dashboard
2. Click "New Project"
3. Select "Deploy from GitHub repo"
4. Choose your repository
5. Railway auto-detects .NET
6. Add MySQL database (click "+ New" → "Database" → "MySQL")
7. Add environment variable `ConnectionStrings__DefaultConnection` with Railway MySQL URL
8. Deploy!

Railway gives you: `https://your-app.up.railway.app`

---

## ✅ **METHOD 3: Render.com (Free Tier)**

### **Step 1: Push to GitHub** (same as Method 2)

### **Step 2: Create Render Account**
- Sign up at: https://render.com/
- Connect GitHub

### **Step 3: Deploy**

1. Click "New +" → "Web Service"
2. Connect your GitHub repo
3. Settings:
   - **Name**: student-event-app
   - **Environment**: Docker
   - **Plan**: Free
4. Create `Dockerfile` (see below)
5. Deploy!

---

## 📦 **Dockerfile (For Railway/Render)**

Create this file in your project root if using Docker deployment:

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["StudentEventManagement/StudentEventManagement.csproj", "StudentEventManagement/"]
RUN dotnet restore "StudentEventManagement/StudentEventManagement.csproj"
COPY . .
WORKDIR "/src/StudentEventManagement"
RUN dotnet build "StudentEventManagement.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StudentEventManagement.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StudentEventManagement.dll"]
```

---

## 🔧 **Important Configuration Changes**

### Update `Program.cs` for production:

Add this before `var app = builder.Build();`:

```csharp
// Configure Kestrel to listen on the port provided by hosting service
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(int.Parse(port));
});
```

---

## 🗄️ **Free Database Options**

1. **FreeMySQLHosting.net** - Free 5MB MySQL
2. **db4free.net** - Free MySQL (200MB)
3. **PlanetScale** - Free tier (5GB)
4. **Railway MySQL** - Included with Railway
5. **Azure MySQL** - Use your $100 student credit

---

## ✅ **Post-Deployment Checklist**

- [ ] App is accessible via URL
- [ ] Database is connected
- [ ] Migrations are applied
- [ ] Login/Signup works
- [ ] Static files (CSS/JS) load correctly
- [ ] HTTPS is enabled (automatic on Azure/Railway/Render)

---

## 🐛 **Troubleshooting**

### App won't start:
```powershell
# Check logs
az webapp log tail --resource-group $resourceGroup --name $appName
```

### Database connection fails:
- Verify connection string
- Check firewall rules (allow Azure services)
- Ensure database exists

### 404 errors:
- Check if static files are published
- Verify routing in Program.cs

---

## 💰 **Cost Monitoring**

### Azure Free Tier Limits:
- 60 CPU minutes/day
- 1GB RAM
- 1GB storage
- After free tier: ~$0/month (with student credit)

### Monitor usage:
```powershell
# Check your credit
az consumption usage list --billing-period-name $(az billing period list --query '[0].name' -o tsv)
```

---

## 📱 **Share Your Live App**

Once deployed, share your URL:
- Azure: `https://your-app-name.azurewebsites.net`
- Railway: `https://your-app.up.railway.app`
- Render: `https://your-app.onrender.com`

---

## 🎓 **Next Steps**

1. Get **custom domain** (free .me domain with GitHub Student Pack)
2. Set up **GitHub Actions** for auto-deployment
3. Add **SSL certificate** (automatic on all platforms)
4. Monitor with **Application Insights** (free on Azure)

---

## 🆘 **Need Help?**

- Azure Docs: https://docs.microsoft.com/azure/app-service/
- Railway Docs: https://docs.railway.app/
- Render Docs: https://render.com/docs

Good luck with your deployment! 🚀

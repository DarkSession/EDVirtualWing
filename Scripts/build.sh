export DOTNET_CLI_TELEMETRY_OPTOUT=1
rm -rf EDVirtualWing
git clone https://github.com/DarkSession/EDVirtualWing.git
cd "EDVirtualWing/src/ED Virtual Wing"
dotnet restore "ED Virtual Wing.csproj"
dotnet build "ED Virtual Wing.csproj" -c Release -o ../../build
dotnet publish "ED Virtual Wing.csproj" -c Release -o ../../publish

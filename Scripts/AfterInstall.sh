cd /home/ec2-user

touch output.txt

sudo wget https://dot.net/v1/dotnet-install.sh >> output.txt
sudo chmod +x ./dotnet-install.sh >> output.txt
sudo ./dotnet-install.sh >> output.txt

dotnet new tool-manifest
dotnet nuget add source -n CommonRepo https://mk-267855555195.d.codeartifact.eu-west-2.amazonaws.com/nuget/Common/v3/index.json
dotnet tool install jsonsettingsupdater
dotnet tool run jsonupdate ImageColourSwap/appsettings.json Integration ProcessingUri
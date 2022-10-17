cd /home/ec2-user

touch output.txt

sudo wget https://dot.net/v1/dotnet-install.sh >> output.txt
sudo chmod +x ./dotnet-install.sh >> output.txt
sudo ./dotnet-install.sh >> output.txt

dotnet tool install jsonsettingsupdater
dotnet new tool-manifest
dotnet nuget add source -n CommonRepo https://mk-267855555195.d.codeartifact.eu-west-2.amazonaws.com/nuget/Common/v3/index.json
sudo yum install dotnet-sdk-6.0 -y
dotnet tool run jsonupdate ImageColourSwap/appsettings.json Integration ProcessingUri
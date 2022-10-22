cd /home/ec2-user

touch output.txt

sudo wget https://dot.net/v1/dotnet-install.sh >> output.txt
sudo chmod +x ./dotnet-install.sh >> output.txt
sudo ./dotnet-install.sh >> output.txt
sudo yum install dotnet-sdk-6.0 -y

export PATH=/home/ec2-user/.dotnet:$PATH
export DOTNET_CLI_HOME=/tmp/

# cd ImageColourSwap

# sudo dotnet tool install -g AWS.CodeArtifact.NuGet.CredentialProvider
# sudo dotnet codeartifact-creds install

# sudo dotnet nuget add source -n CommonRepo https://mk-267855555195.d.codeartifact.eu-west-2.amazonaws.com/nuget/Common/v3/index.json
# sudo dotnet new tool-manifest
# sudo dotnet tool install jsonsettingsupdater
dotnet tool run jsonupdate ImageColourSwap/appsettings.json Integration ProcessingUri
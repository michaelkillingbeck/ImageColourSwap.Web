cd /home/ec2-user

touch output.txt

sudo wget https://dot.net/v1/dotnet-install.sh >> output.txt
sudo chmod +x ./dotnet-install.sh >> output.txt
sudo ./dotnet-install.sh --version latest >> output.txt

export PATH=/home/ec2-user/.dotnet:$PATH
export DOTNET_CLI_HOME=/tmp/
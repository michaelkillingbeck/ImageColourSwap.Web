cd /home/ec2-user/ImageColourSwap

export PATH=/home/ec2-user/.dotnet:$PATH
export DOTNET_CLI_HOME=/tmp/

cp ImageColourSwap.service /etc/systemd/system/
sudo systemctl start ImageColourSwap.service
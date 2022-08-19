cd /home/ec2-user/ImageColourSwap

sudo wget https://dot.net/v1/dotnet-install.sh
sudo chmod +x ./dotnet-install.sh
sudo ./dotnet-install.sh

export PATH=/home/ec2-user/.dotnet:$PATH
export DOTNET_CLI_HOME=/tmp/

cp ImageColourSwap.service /etc/systemd/system/
sudo systemctl start ImageColourSwap.service

cp ImageColourSwap.conf /etc/nginx/conf.d
sudo systemctl restart nginx
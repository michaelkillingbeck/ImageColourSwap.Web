cd /home/ec2-user

cd /home/ec2-user/ImageColourSwap

export PATH=/home/ec2-user/.dotnet:$PATH
export DOTNET_CLI_HOME=/tmp/

cp ImageColourSwap.service /etc/systemd/system/
sudo systemctl start ImageColourSwap.service

cp ImageColourSwap.conf /etc/nginx/conf.d
sudo systemctl restart nginx

sudo certbot --nginx -d ics.integration.michaelkillingbeck.com --non-interactive --agree-tos -m michael@michaelkillingbeck.co.uk
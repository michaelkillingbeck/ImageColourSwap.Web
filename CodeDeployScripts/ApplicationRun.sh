cd /home/ec2-user
sudo chmod -R 700 ImageColourSwap/
sudo chown -R ec2-user ImageColourSwap/
cd /home/ec2-user/ImageColourSwap

export PATH=/home/ec2-user/.dotnet:$PATH
export DOTNET_CLI_HOME=/tmp/

output=$(curl -s https://checkip.amazonaws.com)
hostedZoneID=$(aws route53 list-hosted-zones-by-name |  jq --arg name "integration.michaelkillingbeck.com." -r '.HostedZones | .[] | select(.Name=="\($name)") | .Id')

echo $(jq --arg output "$output" '.Changes[0].ResourceRecordSet.ResourceRecords[0].Value = $output' hosted-zone.json) > hosted-zone.json
aws route53 change-resource-record-sets --hosted-zone-id $hostedZoneID --change-batch file://hosted-zone.json

cp ImageColourSwap.service /etc/systemd/system/
sudo systemctl start ImageColourSwap.service

cp ImageColourSwap.conf /etc/nginx/conf.d
sudo systemctl restart nginx

if [ -f '/etc/letsencrypt/ics.integration.michaelkillingbeck.com/fullchain.pem'];
then
    echo 'certificate exits'
else
    echo 'certificate does not exist'
    sudo certbot --nginx -d ics.integration.michaelkillingbeck.com --non-interactive --agree-tos -m michael@michaelkillingbeck.co.uk
fi
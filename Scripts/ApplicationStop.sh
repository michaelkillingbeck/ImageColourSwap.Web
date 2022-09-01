service_exists() {
    if [[ $(systemctl list-units --all -t service --full --no-legend "$1.service" | sed 's/^\s*//g' | cut -f1 -d' ') == $1.service ]]; then
        return 0
    else
        return 1
    fi
}
if service_exists ImageColourSwap; then
    sudo systemctl stop ImageColourSwap.service
fi
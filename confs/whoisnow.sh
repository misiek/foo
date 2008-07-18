#
set p = /var/www/whonow
while (1)
    cat $p/leszek $p/peep $p/siwy $p/hektor $p/gruby $p/babka > /var/www/whoisnow.html
    sleep 1
end

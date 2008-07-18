#!/usr/bin/perl

$ifconfig=`ifconfig ppp0`;
@fields=split(/\n/,$ifconfig);
@fields=split(/:/,$fields[1]);
@ip=split(/ /,$fields[1]); $ip=$ip[0];
print "* Startuje firewall...\n";
print "* ppp0: $ip\n";
system("/etc/rc.d/rc.firewall $ip");
system("echo $ip > /var/www/currentip");

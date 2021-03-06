#!/usr/bin/env bash

# forked from https://gist.github.com/deinspanjer/9215467
# changed to work with docker-machine instead of boot2docker

usage ()
{  
  cat <<USAGE
docker-fwd -- Helper function to quickly manage port forwards between the docker-machine-vm and the host
Usage: docker-fwd [ -n RULE_NAME ] [ -h HOST_PORT ] [ -p {tcp|udp} ] [ -i HOST_IP ] GUEST_PORT
  or   docker-fwd -d RULE_NAME
  or   docker-fwd -l
  or   docker-fwd -A
  or   docker-fwd -D

Options:
 -n           Use RULE_NAME as the name for the rule -- Defaults to "tcp<GUEST_PORT>" or "udp<GUEST_PORT>"
 -h           Forward HOST_PORT to the guest -- Defaults to the same number as GUEST_PORT
 -p           Forward tcp or udp traffic to GUEST_PORT -- Defaults to "tcp"
 -i           Bind the port forward to HOST_IP -- Defaults to the local only loopback, "127.0.0.1"
 -d           Delete the rule named RULE_NAME from the docker-machine-vm port forwards.
 -l           List the current port forwards defined for docker-machine-vm
 -A           Create forward rules for all the ports that docker uses by default with the -P option (49000-49900)
 -D           Delete all custom rules (i.e. everything except the "docker" and "ssh" rules)
 GUEST_PORT   The listening port inside docker that will receive connections forwarded by the host

Examples:
 docker-fwd 8000
 > Rule tcp8000: tcp port 8000 on host IP 127.0.0.1 forwarded to guest port 8000

 docker-fwd -n fubar -h 8888 8000
 > Rule fubar: tcp port 8888 on host IP 127.0.0.1 forwarded to guest port 8000

 docker-fwd -d fubar
 > Rule fubar deleted

Notes:
 Please don't delete the built in "docker" and "ssh" rules.  Things will break.
USAGE
}


HOST_IP=127.0.0.1
PROTOCOL=tcp
DOCKER_MACHINE_NAME=$(docker-machine active)

list_rules_matching ()
{
    "/c/Program Files/Oracle/VirtualBox/VBoxManage" showvminfo $DOCKER_MACHINE_NAME | grep "NIC [0-9]* Rule([0-9]*): *name = $1"
}

if [ $# -eq 0 ]
then
  usage
  exit 1
fi

while getopts "n:h:p:i:d:lAD" opt
do
    case $opt in
        n)
            RULE_NAME="$OPTARG"
            ;;
        h)
            if [ "$OPTARG" -eq "$OPTARG" ] 2>/dev/null
            then
                HOST_PORT=$OPTARG
            else
                usage
                exit 1
            fi
            ;;
        p)
            if [ "$OPTARG" = "tcp" -o "$OPTARG" = "udp" ]
            then
                PROTOCOL=$OPTARG
            else
                usage
                exit 1
            fi
            ;;
        i)
            HOST_IP="$OPTARG"
            ;;
        d)
            # Check for a numeric name, prefix the tcp default if so
            if [ "$OPTARG" -eq "$OPTARG" ] 2>/dev/null
            then
                RULE_NAME="tcp$OPTARG"
            else
                RULE_NAME="$OPTARG"
            fi
            list_rules_matching $RULE_NAME
            if [ $? -eq 0 ]
            then
                "/c/Program Files/Oracle/VirtualBox/VBoxManage" controlvm $DOCKER_MACHINE_NAME natpf1 delete "$RULE_NAME"
                if [ $? -eq 0 ]
                then
                    echo "Rule deleted."
                else
                    echo "Rule not deleted!"
                fi
            else
                echo "Rule $RULE_NAME not found."
            fi
            exit $?
            ;;
        l)
            list_rules_matching
            exit 0
            ;;
        A)
            echo "Creating 1802 port forwarding rules.  Please wait..."
            for i in {49000..49900}; do
                "/c/Program Files/Oracle/VirtualBox/VBoxManage" controlvm $DOCKER_MACHINE_NAME natpf1 "tcp-port$i,tcp,,$i,,$i"
                "/c/Program Files/Oracle/VirtualBox/VBoxManage" controlvm $DOCKER_MACHINE_NAME natpf1 "udp-port$i,udp,,$i,,$i"
            done
            exit 0
            ;;
        D)
            NUM_RULES=$("/c/Program Files/Oracle/VirtualBox/VBoxManage" showvminfo $DOCKER_MACHINE_NAME | grep 'NIC [0-9]* Rule([0-9]*): *name = ' | grep -o 'name = [^,]*' | grep -cv ' docker\| ssh')
            echo "Deleting $NUM_RULES port forwarding rules.  Please wait..."
            for rule in $("/c/Program Files/Oracle/VirtualBox/VBoxManage" showvminfo $DOCKER_MACHINE_NAME | grep 'NIC [0-9]* Rule([0-9]*): *name = ' | grep -o 'name = [^,]*' | grep -v ' docker\| ssh' | cut -d ' ' -f 3 )
            do
                "/c/Program Files/Oracle/VirtualBox/VBoxManage" controlvm $DOCKER_MACHINE_NAME natpf1 delete "$rule"
            done
            exit 0
            ;;
    esac
done

if [ "${@: -1}" -eq "${@: -1}" ] 2>/dev/null
then
    GUEST_PORT=${@: -1}
else
    usage
    exit 1
fi

if [ -z "$RULE_NAME" ]
then
    RULE_NAME="${PROTOCOL}${GUEST_PORT}"
fi

if [ -z "$HOST_PORT" ]
then
    HOST_PORT="${GUEST_PORT}"
fi


"/c/Program Files/Oracle/VirtualBox/VBoxManage" controlvm $DOCKER_MACHINE_NAME natpf1 "$RULE_NAME,$PROTOCOL,$HOST_IP,$HOST_PORT,,$GUEST_PORT"
if [ $? -eq 0 ]
then
    list_rules_matching $RULE_NAME
    echo "Rule created."
else
    echo "Error creating rule!"
fi
exit $?
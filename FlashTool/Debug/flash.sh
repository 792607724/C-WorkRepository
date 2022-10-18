#!/bin/bash

set -e

function usage()
{
    echo "Usage: $0 <image_dir>"
    exit 1
}

if [ $# -lt 1 ];then
    usage
fi

IMAGE_DIR=$1

if [ ! -d ${IMAGE_DIR} ];then
    echo "${IMAGE_DIR} no such directory!"
    exit 1
fi

fastboot flash IPL0 ${IMAGE_DIR}/ipl_s.bin
fastboot flash IPL_CUST0 ${IMAGE_DIR}/ipl_cust_s.bin
fastboot flash IPL_CUST1 ${IMAGE_DIR}/ipl_cust_s.bin
fastboot flash UBOOT0 ${IMAGE_DIR}/uboot_s.bin
fastboot flash UBOOT1 ${IMAGE_DIR}/uboot_s.bin
fastboot flash KERNEL ${IMAGE_DIR}/kernel
fastboot flash rootfs ${IMAGE_DIR}/rootfs.sqfs
fastboot flash miservice ${IMAGE_DIR}/miservice.ubifs
fastboot flash customer ${IMAGE_DIR}/customer.ubifs

fastboot oem finish_upgrade
fastboot reboot


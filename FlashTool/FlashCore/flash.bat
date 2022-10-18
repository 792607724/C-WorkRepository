@echo START UPGRADE...
fastboot flash IPL0 ipl_s.bin
fastboot flash IPL_CUST0 ipl_cust_s.bin
fastboot flash IPL_CUST1 ipl_cust_s.bin
fastboot flash UBOOT0 uboot_s.bin
fastboot flash UBOOT1 uboot_s.bin
fastboot flash KERNEL kernel
fastboot flash rootfs rootfs.sqfs
fastboot flash miservice miservice.ubifs
fastboot flash customer customer.ubifs
pause
fastboot oem finish_upgrade
fastboot reboot
@echo UPGRADE DONE!!!
pause
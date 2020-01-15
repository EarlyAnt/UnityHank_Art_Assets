adb shell rm -r /storage/sdcard0/Android/data/com.bowhead.hank/files/Model
adb shell mkdir /storage/sdcard0/Android/data/com.bowhead.hank/files/Model
adb shell mkdir /storage/sdcard0/Android/data/com.bowhead.hank/files/Model/role
adb shell mkdir /storage/sdcard0/Android/data/com.bowhead.hank/files/Model/particle
adb push "Android" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/Android"
adb push "Android/role/antler.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/role/antler.ab"
adb push "Android/role/cap.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/role/cap.ab"
adb push "Android/role/christmas hat.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/role/christmas hat.ab"
adb push "Android/role/christmas.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/role/christmas.ab"
adb push "Android/role/cloud_snow.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/role/cloud_snow.ab"
adb push "Android/role/coin hat.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/role/coin hat.ab"
adb push "Android/role/cornu cervi.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/role/cornu cervi.ab"
adb push "Android/role/elk.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/role/elk.ab"
adb push "Android/role/fan wing.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/role/fan wing.ab"
adb push "Android/role/ghost.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/role/ghost.ab"
adb push "Android/role/giftbox.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/role/giftbox.ab"
adb push "Android/role/little mouse.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/role/little mouse.ab"
adb push "Android/role/mouse spring.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/role/mouse spring.ab"
adb push "Android/role/pumpkin.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/role/pumpkin.ab"
adb push "Android/role/snowman.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/role/snowman.ab"
adb push "Android/role/wing.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/role/wing.ab"
adb push "Android/role/wing_snow.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/role/wing_snow.ab"
adb push "Android/particle/center_fx01.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/particle/center_fx01.ab"
adb push "Android/particle/center_fx02.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/particle/center_fx02.ab"
adb push "Android/particle/guangdian_01.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/particle/guangdian_01.ab"
adb push "Android/particle/guangdian_02.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/particle/guangdian_02.ab"
adb push "Android/particle/hezi_fx.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/particle/hezi_fx.ab"
adb push "Android/particle/shandian_fx01.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/particle/shandian_fx01.ab"
adb push "Android/particle/star_fx.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/particle/star_fx.ab"
adb push "Android/particle/tuowei_fx01.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/particle/tuowei_fx01.ab"
adb push "Android/particle/wing01.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/particle/wing01.ab"
adb push "Android/particle/yun.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/particle/yun.ab"

pause
adb reboot
pause

adb shell rm -r /storage/sdcard0/Android/data/com.bowhead.hank/files/Model
adb shell mkdir /storage/sdcard0/Android/data/com.bowhead.hank/files/Model
adb shell mkdir /storage/sdcard0/Android/data/com.bowhead.hank/files/Model/role
adb shell mkdir /storage/sdcard0/Android/data/com.bowhead.hank/files/Model/particle
adb push "Android" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/Android"
cd Android
adb push "role/antler.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/role/antler.ab"
adb push "role/cap.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/role/cap.ab"
adb push "role/christmas hat.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/role/christmas hat.ab"
adb push "role/christmas.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/role/christmas.ab"
adb push "role/cloud_snow.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/role/cloud_snow.ab"
adb push "role/coin hat.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/role/coin hat.ab"
adb push "role/cornu cervi.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/role/cornu cervi.ab"
adb push "role/elk.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/role/elk.ab"
adb push "role/fan wing.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/role/fan wing.ab"
adb push "role/ghost.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/role/ghost.ab"
adb push "role/giftbox.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/role/giftbox.ab"
adb push "role/little mouse.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/role/little mouse.ab"
adb push "role/mouse spring.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/role/mouse spring.ab"
adb push "role/pumpkin.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/role/pumpkin.ab"
adb push "role/snowman.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/role/snowman.ab"
adb push "role/wing.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/role/wing.ab"
adb push "role/wing_snow.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/role/wing_snow.ab"
adb push "particle/center_fx01.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/particle/center_fx01.ab"
adb push "particle/center_fx02.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/particle/center_fx02.ab"
adb push "particle/guangdian_01.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/particle/guangdian_01.ab"
adb push "particle/guangdian_02.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/particle/guangdian_02.ab"
adb push "particle/hezi_fx.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/particle/hezi_fx.ab"
adb push "particle/shandian_fx01.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/particle/shandian_fx01.ab"
adb push "particle/star_fx.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/particle/star_fx.ab"
adb push "particle/tuowei_fx01.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/particle/tuowei_fx01.ab"
adb push "particle/wing01.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/particle/wing01.ab"
adb push "particle/yun.ab" "/storage/sdcard0/Android/data/com.bowhead.hank/files/Model/particle/yun.ab"

pause
adb reboot
pause

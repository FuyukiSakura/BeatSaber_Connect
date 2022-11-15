// Based on https://github.com/rynan4818/obs-control/blob/main/js/obs-control.js

const obs_game_scene_name = 'BS-Game';        //ゲームシーン名
const obs_menu_scene_name = 'BS-Menu';        //メニューシーン名
const obs_game_event_delay = 0;                //ゲームシーン開始タイミングを遅らせる場合に遅らせるミリ秒を設定して下さい。タイミングを早めること（マイナス値）はできません。[0の場合は無効]
const obs_menu_event_delay = 0;                //ゲームシーン終了(メニューに戻る)タイミングを遅らせる場合に遅らせるミリ秒を設定して下さい。タイミングを早めること（マイナス値）はできません。[0の場合は無効]
const obs_menu_event_switch = false;           //[true/false]ゲームシーン終了タイミングをfinish/failした瞬間に変更する場合は true にします。約1秒程度早まりますのでobs_menu_event_delayと合わせて終了タイミングの微調整に使えます。
const obs_start_scene_duration = 2;              //ゲームシーンに切り替える前に開始シーンを表示する時間(秒単位[小数3位までOK]) [0の場合は無効]
const obs_start_scene_name = 'BS-Start';     //開始シーン名  ※使用時はobs_start_scene_durationの設定要
const obs_pause_scene_duration = 0;              //Pause(ポーズ)してメニューに戻る場合にメニューシーンに切替わる前に終了シーンを表示する時間(秒単位[小数3位までOK]) [0の場合は無効]
const obs_pause_scene_name = 'BS-Pause';     //Pause(ポーズ)用終了シーン名  ※使用時はobs_pause_scene_durationの設定要
const obs_recording_check = false;          //[true/false]trueにするとゲームシーン開始時に録画状態をチェックする。

let obs_browser_check = false;
let obs_now_scene;
let obs_bs_menu_flag = true;
let obs_end_event = '';
let obs_timeout_id;
let song_scene_list = false;
let song_scene_time_index = 0;
let song_scene_time_list = false;
let song_scene_end_change = true;
let song_scene_game_name = false;
let song_scene_start_name = false;
let song_scene_start_duration = false;
let song_scene_menu_name = false;
let song_scene_end_name = false;
let song_scene_end_duration = false;

window.obsstudio.getControlLevel(function (level) {
    //Level - The level of permissions. 0 for NONE, 1 for READ_OBS (OBS data), 2 for READ_USER (User data), 3 for BASIC, 4 for ADVANCED and 5 for ALL
    if (level >= 4) {
        obs_browser_check = true;
    } else {
        console.log("Webpage control permissions are missing");
        var element = document.createElement("div");
        element.innerHTML =
            "Webpage control permissions are missing. <br>Please change to ADVANCED or higher.<br><br>ブラウザソースのプロパティでページ権限が不足しています。<br>OBSへの高度なアクセス以上に設定して下さい。";
        element.style =
            "display: block;font-size: 20px;font-weight: 700;line-height: 41px; letter-spacing: 2px; margin: 0 0 0 10px; color: red";
        document.body.appendChild(element);
    }
});

function obs_scene_change(scene_name) {
    if (!obs_browser_check) return;
    window.obsstudio.setCurrentScene(scene_name);
}

window.obs_menu_scene_change = () => {
    console.log("Menu schene changed");
    let menu_scene_name = obs_menu_scene_name;
    if (song_scene_menu_name !== false) menu_scene_name = song_scene_menu_name;
    obs_scene_change(menu_scene_name);
};

function obs_game_scene_change() {
    let game_scene_name = obs_game_scene_name;
    if (song_scene_game_name !== false) game_scene_name = song_scene_game_name;
    obs_scene_change(game_scene_name);
}

window.obs_start_scene_change = () => {
    let start_scene_duration = obs_start_scene_duration;
    let start_scene_name = obs_start_scene_name;
    let game_scene_name = obs_game_scene_name;
    if (song_scene_start_duration !== false) start_scene_duration = song_scene_start_duration;
    if (song_scene_start_name !== false) start_scene_name = song_scene_start_name;
    if (song_scene_game_name !== false) game_scene_name = song_scene_game_name;

    if (start_scene_duration > 0) {
        obs_scene_change(start_scene_name);
        obs_timeout_id = setTimeout(obs_game_scene_change, start_scene_duration * 1000);
    } else {
        obs_scene_change(game_scene_name);
    }
};

window.obs_end_scene_change = (obj) => {
    let quitScene = obj.scene;
    let duration = obj.duration;
    obs_scene_change(quitScene);

    let menu_scene_name = obs_menu_scene_name;
    if (song_scene_menu_name !== false) menu_scene_name = song_scene_menu_name;
    if (duration > 0) {
        obs_timeout_id = setTimeout(obs_menu_scene_change, duration * 1000);
    } else {
        obs_scene_change(menu_scene_name);
    }
};

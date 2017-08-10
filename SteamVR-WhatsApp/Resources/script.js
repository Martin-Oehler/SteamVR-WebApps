var login_page_html = ""

function onPageLoaded() {
    if (checkLoginPage()) {
        console.log("we are not logged in");
        // we are not logged in 
        openLoginPopup(); // TODO: somehow close it again!
        
    } else {
        console.log("we are logged in");
        // we are logged in
        setTimeout(checkNewMessages, 1000); // check for new messages every second
    }
}

// check if we are on login-page
function checkLoginPage() {
    var elements = document.getElementsByClassName("avatar-body");
    return elements.length == 0;
}

// get qr code as base64 string
function getQrCodeString() {
    var imgs = document.querySelectorAll(".qrcode > img");
    if (imgs.length == 0) {
        return "";
    }
    return imgs[0].src;
}

// open new popup so user can scan on PC
function openLoginPopup() {
    var qrstring = getQrCodeString();
    if (qrstring != "") {
        console.log("opening popup");
        var win = window.open("", "WhatsApp Login", "toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, width=1200, height=600");
        win.document.body.innerHTML = login_page_html;
        qrcode_div = win.document.getElementById("qrcode");
        qrcode_div.innerHTML = "<img src='" + qrstring + "' style='display:block;'>"
    } else {
        setTimeout(function () {
            console.log("didn't find qrcode, trying again in 1s..");
            openLoginPopup()
        }, 1000);
    }
}

// check for new messages
function checkNewMessages() {
    console.log("checking for new messages");
    var unread_msgs = document.getElementsByClassName("unread");
    console.log("Found " + unread_msgs.length + " unread messages");
    for (unread of unread_msgs) {
        var title_div = unread.getElementsByClassName("chat-title");
        if (title_div.length == 0) continue;

        var title_span = title_div[0].getElementsByTagName("span");
        if (title_span.length == 0) continue;

        var title = title_span[0].title;

        var last_msg_div = unread.getElementsByClassName("last-msg");
        if (last_msg_div.length == 0) continue;
        var message = last_msg_div[0].title;

        var sender_span = last_msg_div[0].getElementsByClassName("sender");
        var sender = "";
        if (sender_span.length != 0) {
            var text_span = last_msg_div[0].getElementsByTagName("span");
            if (text_span.length != 0) {
                sender = text_span[0].innerText;
            }
        }

        messageNotification(title, sender, text);

    }
}

// send a notification with sender and message text
function messageNotification(title, sender, text) {
    var message_string;
    message_string = "title\n";
    if (sender != "") {
        message_string += sender + ": ";
    }
    message_string += text;
    notifications.sendNotification(message_string);
}
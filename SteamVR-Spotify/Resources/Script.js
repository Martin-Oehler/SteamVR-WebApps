var user = '[USER]'
var pwd = '[PWD]'

account_link = document.getElementById('has-account')
if (account_link) {
    account_link.click()

    login_user = document.getElementById('login-usr')
    if (login_user)
    {
        login_user.value = user
        document.getElementById('login-pass').value = pwd
    }
} else {
    var last_artist = ''
    var last_name = ''

    function track_notification( )
    {
        var artist_dom = document.getElementsByClassName('track-info__artists')[0]
        var artist = 'Unknown'
        if (artist_dom) {
            artist = artist_dom.firstChild.firstChild.firstChild.textContent
        } else {
            return
        }

        var name_dom = document.getElementsByClassName('track-info__name')[0]
        var name = 'Unknown'
        if (name_dom) {
            name = name_dom.firstChild.firstChild.firstChild.textContent
        } else {
            return
        }
        if (last_artist !== artist || last_name !== name) {
            notifications.sendNotification('Now playing: ' + artist + ' - ' + name)
            last_artist = artist
            last_name = name
        }
    }

    setInterval(track_notification, 2000);
}

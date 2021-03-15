import QtQuick 2.15
import QtQuick.Controls.Material 2.12
import QtQuick.Controls 2.15
import Mpv 1.0
import Room 1.0

Item {
    property string serverIp
    property int serverPort
    property string username

    anchors.fill: parent

    Rectangle {
        id: connectingScreen

        anchors.fill: parent

        color: Material.color(Material.BlueGrey, Material.Shade700)
    }

    Popup {
        id: popup

        anchors.centerIn: parent

        width: 300
        height: 200
        z: 10

        modal: true
        focus: true
        closePolicy: Popup.CloseOnEscape | Popup.CloseOnPressOutside

        contentItem: Text {
            horizontalAlignment: Text.AlignHCenter
            verticalAlignment: Text.AlignVCenter

            color: Material.foreground

            text: qsTr("Could not connect to room")
        }

        onClosed: {
            mainWindow.disconnectFromRoom()
        }
    }

    Rectangle {
        color: Material.background
        anchors.top: parent.top
        anchors.bottom: parent.bottom
        anchors.left: chat_box.right
        anchors.right: parent.right

        visible: connectingScreen.visible ? false : true

        Mpv {
            id: player

            anchors.top: parent.top
            anchors.bottom: inputqwe.top
            anchors.left: parent.left
            anchors.right: parent.right
        }
        Rectangle {
            color: Material.background
            id: inputqwe

            anchors.bottom: parent.bottom
            anchors.left: parent.left
            anchors.right: parent.right

            visible: connectingScreen.visible ? false : true

            height: parent.height / 8

            border.width: 2
//            border.color: "yellow"

            Button {
                id: bbbb
                anchors.top: parent.top
                anchors.bottom: parent.bottom
                anchors.left: parent.left

                property var pause: false

                text: "Pause"

                onClicked: {
                    pause = !pause
                    room.setPause(pause)
//                    room.executeApiMethod(Codes.VideoPauseCycle)
                }
            }

            TextInput {
                anchors.top: parent.top
                anchors.bottom: parent.bottom
                anchors.left: bbbb.right
                anchors.right: parent.right

                onAccepted: {
//                    let d = {
//                        "url": text
//                    }
//                    room.executeApiMethod(Codes.VideoSetUrl, d)
                    room.setUrl(text)
                    clear()
                }
            }
        }
    }

    Rectangle {
        id: chat_box

        color: Material.background
        anchors.top: parent.top
        anchors.bottom: parent.bottom
        anchors.left: parent.left

        visible: connectingScreen.visible ? false : true

        anchors.rightMargin: 5
        implicitWidth: 50
        width: parent.width * 0.35

        border.width: 3

        ListView {
            id: chat_messages

            anchors.top: parent.top
            anchors.bottom: chat_box_input.top

            leftMargin: 5
            rightMargin: 5
            width: parent.width
            verticalLayoutDirection: ListView.TopToBottom
            spacing: 4

            delegate: Text {
                text: chatMessageText

                color: Material.foreground

            }

            model: ListModel {
                id: chatModel
            }
        }
        Rectangle {
            id: chat_box_input

            color: Material.background
            anchors.bottom: parent.bottom
            anchors.left: parent.left
            anchors.right: parent.right

            width: parent.width
            height: parent.height / 8

            border.width: 1

            TextInput {
                anchors.fill: parent

                anchors.leftMargin: 5
                anchors.rightMargin: 5

                onAccepted: {
                    room.sendChatMessage(text)
                    clear()
                }
            }
        }
    }

    Room {
        id: room

        Component.onCompleted: function() {
            room.connect(serverIp, serverPort, username)

            room.userConnectedSignal.connect(function(username) {
                chatModel.append({chatMessageText: username + " connected."})
            })

            room.chatMessageSignal.connect(function(username, text) {
                chatModel.append({chatMessageText: username + ": " + text})
            })

            room.urlChangedSignal.connect(function(url) {
                console.log(url)
                player.open(url)
            })

            room.pauseChangedSignal.connect(function(pause) {
                player.pauseCycle()
            })

//            room.videoPauseChanged.connect(function () {
//                player.pauseCycle()
//            })
//            room.videoUrlChanged.connect(function() {
//                player.open(room.videoUrl)
//            })

            connectingScreen.visible = false
        }
    }

  function free() {
      player.destroy()
      room.disconnect()
  }
}

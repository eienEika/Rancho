import QtQuick 2.15
import QtQuick.Controls 2.15
import QtQuick.Layouts 1.15
import QtQuick.Controls.Material 2.12

ApplicationWindow {
    id: mainWindow
    width: 1366
    height: 768
    title: qsTr("Rancho")
    visible: true

    property var mRoom

    Rectangle {
        id: mtop

        anchors.top: parent.top
        anchors.left: parent.left
        anchors.right: parent.right

        height: 1

        color: "black"
    }
    Rectangle {
        id: mbot

        anchors.bottom: parent.bottom
        anchors.left: parent.left
        anchors.right: parent.right

        height: 1

        color: "black"
    }

    header: GridLayout {
        rows: 1
        columns: 2
        width: parent.width

        RowLayout {
            Layout.leftMargin: 30

            Button {
                id: btnHeaderDisconnect

                visible: false
                text: "Disconnect"

                onClicked: {
                    disconnectFromRoom()
                }
            }
            Button {
                id: btnHeaderNew

                text: "New"
            }
            Button {
                id: btnHeaderConnect

                text: "Direct connect"

                onClicked: {
                    connectPopup.open()
                }
            }
        }
        RowLayout {
            Layout.rightMargin: 30
            Layout.alignment: Qt.AlignRight

            Button {
                id: btn_setting

                text: "Settings"
            }
        }
    }

    footer: GridLayout {
        rows: 1
        columns: 2

        RowLayout {
            Layout.leftMargin: 30

            Text {
                color: Material.color(Material.Pink)
                text: "Theater"
            }
        }
    }

    Item {
        id: mbody

        anchors.top: mtop.top
        anchors.bottom: mbot.bottom
        anchors.left: parent.left
        anchors.right: parent.right
    }

    Popup {
        id: connectPopup

        anchors.centerIn: parent

        width: 300
        height: 200
        z: 10

        modal: true
        focus: true
        closePolicy: Popup.CloseOnEscape | Popup.CloseOnPressOutside

        contentItem: Item {
            TextField {
                id: connectIpInput

                anchors.top: parent.top
                anchors.left: parent.left

                height: parent.height / 2
                width: parent.width / 2

                placeholderText: "Ip"
            }
            TextField {
                id: connectPortInput

                anchors.top: parent.top
                anchors.right: parent.right

                height: parent.height / 2
                width: parent.width / 2

                placeholderText: "Port"
            }
            TextField {
                id: connectUsernameInput

                anchors.top: connectIpInput.bottom
                anchors.left: parent.left
                anchors.right: parent.right

                placeholderText: "Username"
            }
            Button {
                anchors.top: connectUsernameInput.bottom
                anchors.bottom: parent.bottom
                anchors.left: parent.left
                anchors.right: parent.right

                text: "Connect"

                onClicked: makeRoom()
            }
        }
    }

    function makeRoom() {
        btnHeaderNew.visible = false
        btnHeaderConnect.visible = false
        let o = {
            "serverIp": connectIpInput.text,
            "serverPort": connectPortInput.text,
            "username": connectUsernameInput.text
        }
        mRoom = Qt.createComponent("room.qml").createObject(mbody, o)
        btnHeaderDisconnect.visible = true
        connectPopup.close()
    }

    function disconnectFromRoom() {
        btnHeaderDisconnect.visible = false
        mRoom.free()
        mRoom.destroy()
        btnHeaderNew.visible = true
        btnHeaderConnect.visible = true
    }
}

QT += quick qml widgets

CONFIG += c++11

HEADERS += \
    mpvobject.h \
    mpvrenderer.h

SOURCES += \
        main.cpp \
        mpvobject.cpp \
        mpvrenderer.cpp \
        qthelper.hpp

RESOURCES += qml.qrc

qnx: target.path = /tmp/$${TARGET}/bin
else: unix:!android: target.path = /opt/$${TARGET}/bin
!isEmpty(target.path): INSTALLS += target

include (qmlnet-native/Hosting.pri)
include (qmlnet-native/QmlNet.pri)

#win32: LIBS += -L$$PWD/../../Toolkits/mpv-dev-20170212/32 -llibmpv.dll
unix: LIBS += -lmpv

DEFINES += "NET_ROOT=\"\\\"$$PWD/../build/net\\\"\""

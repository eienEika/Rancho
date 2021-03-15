#include <QApplication>
#include <QDir>
#include <QQmlApplicationEngine>
#include "Hosting/CoreHost.h"
#include "mpvobject.h"

static int runCallback(QGuiApplication* app, QQmlApplicationEngine* engine)
{
    engine->load(QUrl(QStringLiteral("qrc:/qml/main.qml")));

    if (engine->rootObjects().isEmpty())
    {
        return -1;
    }

    return app->exec();
}

int main(int argc, char* argv[])
{
    QCoreApplication::setAttribute(Qt::AA_EnableHighDpiScaling);
    QApplication app(argc, argv);
    std::setlocale(LC_NUMERIC, "C");

    qmlRegisterType<MpvObject>("Mpv", 1, 0, "Mpv");
//    qmlRegisterType<ApiCodes>("Codes", 1, 0, "Codes");

    QQmlApplicationEngine engine;

    QString netDll = NET_ROOT;
    netDll.append(QDir::separator());
    netDll.append("Rancho.Client.dll");

    CoreHost::RunContext runContext;
    runContext.hostFxrContext = CoreHost::findHostFxr();
    runContext.managedExe = netDll;
    runContext.entryPoint = runContext.hostFxrContext.dotnetRoot;
    runContext.entryPoint.append(CORECLR_DOTNET_EXE_NAME);

    return CoreHost::run(app, engine, runCallback, runContext);
}

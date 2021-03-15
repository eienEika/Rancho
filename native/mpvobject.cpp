#include "mpvobject.h"

#include <QQuickWindow>
#include "qthelper.hpp"
#include "mpvrenderer.h"

MpvObject::MpvObject(QQuickItem* parent)
    : QQuickFramebufferObject(parent)
    , mpv{mpv_create()}
    , mpvGl(nullptr)
{
    if (!mpv)
    {
        throw std::runtime_error("could not create mpv context");
    }

    if (mpv_initialize(mpv) < 0)
    {
        throw std::runtime_error("could not initialize mpv context");
    }

    mpv::qt::set_option_variant(mpv, "hwdec", "auto");

    connect(this, &MpvObject::onUpdate, this, &MpvObject::doUpdate, Qt::QueuedConnection);
}

MpvObject::~MpvObject()
{
    destroy();
}

void MpvObject::command(const QVariant& params)
{
    qDebug() << "Command" << params;
    mpv::qt::command(mpv, params);
}

QQuickFramebufferObject::Renderer* MpvObject::createRenderer() const
{
    window()->setPersistentOpenGLContext(true);
    window()->setPersistentSceneGraph(true);
    return new MpvRenderer(const_cast<MpvObject*>(this));
}

void MpvObject::destroy()
{
    if (mpvGl)
    {
        mpv_render_context_free(mpvGl);
    }
    mpv_terminate_destroy(mpv);
}

void MpvObject::doUpdate()
{
    update();
}

void MpvObject::on_update(void* ctx)
{
    MpvObject* self = (MpvObject*)ctx;
    emit self->onUpdate();
}

void MpvObject::open(const QString& url)
{
    QStringList c;
    c << "loadfile" << url;
    command(c);
}

void MpvObject::pauseCycle()
{
    QStringList c;
    c << "cycle" << "pause";
    command(c);
}

void MpvObject::setProperty(const QString& name, const QVariant& value)
{
    mpv::qt::set_property(mpv, name, value);
}

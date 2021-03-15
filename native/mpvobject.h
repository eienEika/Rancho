#ifndef MPVOBJECT_H
#define MPVOBJECT_H

#include <QQuickFramebufferObject>
#include "mpv/client.h"
#include "mpv/render_gl.h"

class MpvRenderer;

class MpvObject : public QQuickFramebufferObject
{
    Q_OBJECT
    QML_ELEMENT

public:
    MpvObject(QQuickItem* parent = 0);
    virtual ~MpvObject();
    virtual Renderer* createRenderer() const;
    static void on_update(void* ctx);

public slots:
    void destroy();
    void open(const QString& url);
    void pauseCycle();
    void setProperty(const QString& name, const QVariant& value);

signals:
    void onUpdate();

private slots:
    void doUpdate();

private:
    friend class MpvRenderer;
    mpv_handle* mpv;
    mpv_render_context* mpvGl;
    void command(const QVariant& params);
};

#endif // MPVOBJECT_H

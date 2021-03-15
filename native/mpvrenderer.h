#ifndef MPVRENDERER_H
#define MPVRENDERER_H

#include "mpvobject.h"

class MpvRenderer : public QQuickFramebufferObject::Renderer
{
public:
    MpvRenderer(MpvObject* mpvObj);
    virtual ~MpvRenderer();
    QOpenGLFramebufferObject* createFramebufferObject(const QSize& size);
    void render();

private:
    MpvObject* mpvObj;
};

#endif // MPVRENDERER_H

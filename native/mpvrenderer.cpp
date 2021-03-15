#include "mpvrenderer.h"

#include <QOpenGLContext>
#include <QOpenGLFramebufferObject>
#include <QQuickWindow>

static void onMpvEvents(void* ctx)
{
    Q_UNUSED(ctx);
}

static void* getProcAddress(void* ctx, const char* name)
{
    Q_UNUSED(ctx);

    QOpenGLContext* glctx = QOpenGLContext::currentContext();

    if (!glctx)
    {
        return nullptr;
    }

    return reinterpret_cast<void*>(glctx->getProcAddress(QByteArray(name)));
}

void onMpvRedraw(void* ctx)
{
    MpvObject::on_update(ctx);
}

MpvRenderer::MpvRenderer(MpvObject* mpvObj_) : mpvObj(mpvObj_)
{
    mpv_set_wakeup_callback(mpvObj->mpv, onMpvEvents, this);
}

MpvRenderer::~MpvRenderer()
{
}

QOpenGLFramebufferObject* MpvRenderer::createFramebufferObject(const QSize &size)
{
    if (!mpvObj->mpvGl)
    {
        mpv_opengl_init_params glInitParams{getProcAddress, nullptr, nullptr};

        mpv_render_param params[]
        {
            {MPV_RENDER_PARAM_API_TYPE, const_cast<char*>(MPV_RENDER_API_TYPE_OPENGL)},
            {MPV_RENDER_PARAM_OPENGL_INIT_PARAMS, &glInitParams},
            {MPV_RENDER_PARAM_INVALID, nullptr}
        };

        if (mpv_render_context_create(&mpvObj->mpvGl, mpvObj->mpv, params) < 0)
        {
            throw std::runtime_error("failed to initialize mpv GL context");
        }

        mpv_render_context_set_update_callback(mpvObj->mpvGl, onMpvRedraw, mpvObj);
    }

    return QQuickFramebufferObject::Renderer::createFramebufferObject(size);
}

void MpvRenderer::render()
{
    mpvObj->window()->resetOpenGLState();

    QOpenGLFramebufferObject* fbo = framebufferObject();
    mpv_opengl_fbo mpfbo{.fbo = static_cast<int>(fbo->handle()), .w = fbo->width(), .h = fbo->height(), .internal_format = 0};
    int flip_y{0};

    mpv_render_param params[] =
    {
        {MPV_RENDER_PARAM_OPENGL_FBO, &mpfbo},
        {MPV_RENDER_PARAM_FLIP_Y, &flip_y},
        {MPV_RENDER_PARAM_INVALID, nullptr}
    };

    mpv_render_context_render(mpvObj->mpvGl, params);

    mpvObj->window()->resetOpenGLState();
}

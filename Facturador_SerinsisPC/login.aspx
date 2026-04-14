<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="Facturador_SerinsisPC.login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Acceso | SERINSIS PC</title>
    <link rel="shortcut icon" href="img/logo.ico" type="image/x-icon" />
    <link href="Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.8.1/css/all.css" />
    <script src="js/jquery-3.6.0.js"></script>
    <script src="js/aletrs.js"></script>
    <script src="js/swalert.js"></script>
    <style>
        body {
            min-height: 100vh;
            margin: 0;
            font-family: 'Bahnschrift';
            background:
                radial-gradient(circle at top left, rgba(98, 175, 226, 0.18), transparent 20%),
                linear-gradient(135deg, #fafdff 0%, #edf6ff 45%, #e9f3ff 100%);
            color: #22305e;
        }

        .login-shell {
            min-height: 100vh;
            display: grid;
            grid-template-columns: 1.1fr .9fr;
        }

        .login-brand {
            padding: 3rem 3.5rem;
            background: linear-gradient(180deg, #211b84 0%, #2a22a6 42%, #3a33bc 100%);
            color: #fff;
            display: flex;
            flex-direction: column;
            justify-content: space-between;
        }

        .login-brand__top {
            max-width: 540px;
        }

        .login-brand__logo {
            width: 94px;
            height: 94px;
            object-fit: contain;
            border-radius: 24px;
            background: rgba(255,255,255,.12);
            padding: .55rem;
            margin-bottom: 1.25rem;
        }

        .login-brand__title {
            font-size: 2.4rem;
            font-weight: 700;
            margin-bottom: .55rem;
        }

        .login-brand__text {
            font-size: 1.02rem;
            color: rgba(255,255,255,.82);
            max-width: 470px;
        }

        .login-panel-wrap {
            display: flex;
            align-items: center;
            justify-content: center;
            padding: 2rem;
        }

        .login-panel {
            width: 100%;
            max-width: 490px;
            background: rgba(255,255,255,.94);
            border: 1px solid #cfe2f3;
            border-radius: 24px;
            box-shadow: 0 28px 60px rgba(42, 34, 166, 0.16);
            overflow: hidden;
        }

        .login-panel__header {
            padding: 1.35rem 1.4rem 1rem;
            background: linear-gradient(135deg, #2a22a6 0%, #62afe2 100%);
            color: #fff;
        }

        .login-panel__title {
            margin: 0;
            font-size: 1.4rem;
            font-weight: 700;
        }

        .login-panel__subtitle {
            margin-top: .35rem;
            color: rgba(255,255,255,.84);
            font-size: .92rem;
        }

        .login-panel__body {
            padding: 1.4rem;
        }

        .login-tabs {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: .55rem;
            margin-bottom: 1rem;
        }

        .login-tab {
            border: 1px solid #cfe2f3;
            border-radius: 14px;
            background: #f4f9ff;
            color: #2a22a6;
            font-weight: 700;
            padding: .65rem .8rem;
            cursor: pointer;
            text-align: center;
        }

        .login-tab--active {
            background: linear-gradient(135deg, #3c35c6 0%, #2a22a6 100%);
            color: #fff;
            border-color: transparent;
            box-shadow: 0 12px 24px rgba(42, 34, 166, 0.14);
        }

        .login-section { display: none; }
        .login-section--active { display: block; }

        .login-label {
            color: #2a22a6;
            font-weight: 700;
            font-size: .9rem;
            margin-bottom: .35rem;
        }

        .login-input {
            min-height: 46px;
            border: 1px solid #cfe2f3;
            border-radius: 14px;
            color: #243a60;
            box-shadow: none;
        }

        .login-input:focus {
            border-color: #62afe2;
            box-shadow: 0 0 0 .16rem rgba(98, 175, 226, 0.18);
        }

        .login-btn {
            min-height: 48px;
            border: 0;
            border-radius: 14px;
            font-weight: 700;
            background: linear-gradient(135deg, #3c35c6 0%, #2a22a6 100%);
            box-shadow: 0 12px 24px rgba(42, 34, 166, 0.18);
        }

        .login-help {
            margin-top: .9rem;
            color: #61779c;
            font-size: .9rem;
            line-height: 1.5;
        }

        @media (max-width: 991.98px) {
            .login-shell { grid-template-columns: 1fr; }
            .login-brand { padding: 2rem 1.3rem; }
            .login-panel-wrap { padding: 1rem 1rem 2rem; }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-shell">
            <section class="login-brand">
                <div class="login-brand__top">
                    <img src="img/logo.png" alt="SERINSIS PC" class="login-brand__logo" />
                    <div class="login-brand__title">SERINSIS PC</div>
                    <div class="login-brand__text">
                        Acceso administrativo protegido para la gestion de clientes, facturacion, cobros e ingresos.
                    </div>
                </div>
                <div class="login-brand__text">
                    Este acceso es exclusivo para administradores autorizados.
                </div>
            </section>

            <section class="login-panel-wrap">
                <div class="login-panel">
                    <div class="login-panel__header">
                        <h1 class="login-panel__title">Acceso Administrativo</h1>
                        <div class="login-panel__subtitle">Ingresa o recupera acceso con tu usuario y WhatsApp autorizado</div>
                    </div>
                    <div class="login-panel__body">
                        <div class="login-tabs">
                            <button type="button" id="tabLogin" class="login-tab login-tab--active" onclick="mostrarPanelLogin('login')">Iniciar Sesion</button>
                            <button type="button" id="tabRecuperar" class="login-tab" onclick="mostrarPanelLogin('recuperar')">Recuperar Acceso</button>
                        </div>

                        <div id="panelLogin" class="login-section login-section--active">
                            <div class="form-group">
                                <label class="login-label">Usuario</label>
                                <asp:TextBox runat="server" ID="txtUsuario" CssClass="form-control login-input" />
                            </div>
                            <div class="form-group">
                                <label class="login-label">Clave</label>
                                <asp:TextBox runat="server" ID="txtClave" TextMode="Password" CssClass="form-control login-input" />
                            </div>
                            <asp:Button runat="server" ID="btnIngresar" Text="Ingresar al Panel" CssClass="btn btn-primary btn-block login-btn" OnClick="btnIngresar_Click" />
                            <div class="login-help">
                                Este acceso es exclusivo para administradores creados directamente en base de datos.
                            </div>
                        </div>

                        <div id="panelRecuperar" class="login-section">
                            <div class="form-group">
                                <label class="login-label">Usuario</label>
                                <asp:TextBox runat="server" ID="txtUsuarioRecuperar" CssClass="form-control login-input" />
                            </div>
                            <div class="form-group">
                                <label class="login-label">WhatsApp de Recuperacion</label>
                                <asp:TextBox runat="server" ID="txtWhatsAppRecuperar" CssClass="form-control login-input" />
                            </div>
                            <div class="form-group">
                                <asp:Button runat="server" ID="btnEnviarCodigo" Text="Enviar Codigo por WhatsApp" CssClass="btn btn-outline-primary btn-block" Style="min-height:46px;border-radius:14px;font-weight:700;" OnClick="btnEnviarCodigo_Click" />
                            </div>
                            <div class="form-group">
                                <label class="login-label">Codigo de Verificacion</label>
                                <asp:TextBox runat="server" ID="txtCodigoRecuperacion" CssClass="form-control login-input" MaxLength="6" />
                            </div>
                            <div class="form-group">
                                <label class="login-label">Nueva Clave</label>
                                <asp:TextBox runat="server" ID="txtClaveNueva" TextMode="Password" CssClass="form-control login-input" />
                            </div>
                            <div class="form-group">
                                <label class="login-label">Confirmar Nueva Clave</label>
                                <asp:TextBox runat="server" ID="txtClaveNueva2" TextMode="Password" CssClass="form-control login-input" />
                            </div>
                            <asp:Button runat="server" ID="btnRecuperar" Text="Validar Codigo y Actualizar Clave" CssClass="btn btn-primary btn-block login-btn" OnClick="btnRecuperar_Click" />
                            <div class="login-help">
                                El sistema enviara un codigo temporal de un solo uso al WhatsApp autorizado. Ese codigo vence en pocos minutos.
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </form>

    <script>
        function mostrarPanelLogin(panel) {
            var esLogin = panel === 'login';
            document.getElementById('panelLogin').classList.toggle('login-section--active', esLogin);
            document.getElementById('panelRecuperar').classList.toggle('login-section--active', !esLogin);
            document.getElementById('tabLogin').classList.toggle('login-tab--active', esLogin);
            document.getElementById('tabRecuperar').classList.toggle('login-tab--active', !esLogin);
        }
    </script>
</body>
</html>

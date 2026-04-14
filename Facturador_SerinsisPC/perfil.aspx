<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="perfil.aspx.cs" Inherits="Facturador_SerinsisPC.perfil" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .perfil-page { display: flex; flex-direction: column; gap: .9rem; }
        .perfil-header { display: flex; align-items: center; justify-content: space-between; gap: 1rem; }
        .perfil-header__title { display: flex; align-items: center; gap: .7rem; color: #2a22a6; margin: 0; font-size: 2rem; font-weight: 700; }
        .perfil-header__title i { color: #62afe2; font-size: 1.7rem; }
        .perfil-divider { height: 1px; background: linear-gradient(90deg, #c9e3f5 0%, rgba(201, 227, 245, 0) 100%); }
        .perfil-grid { display: grid; grid-template-columns: minmax(280px, .8fr) minmax(420px, 1.2fr); gap: 1rem; }
        .perfil-card { background: linear-gradient(135deg, #fbfdff 0%, #f3f9ff 100%); border: 1px solid #cfe2f3; border-radius: 16px; padding: .95rem 1rem; box-shadow: 0 10px 24px rgba(42, 34, 166, 0.05); }
        .perfil-card__title { color: #2a22a6; font-weight: 700; font-size: .95rem; margin-bottom: .25rem; }
        .perfil-card__text { color: #61779c; font-size: .92rem; margin-bottom: .8rem; }
        .perfil-meta { display: grid; gap: .8rem; }
        .perfil-meta__item { background: #fff; border: 1px solid #d8e6f4; border-radius: 14px; padding: .75rem .85rem; }
        .perfil-meta__label { display:block; font-size:.78rem; color:#61779c; font-weight:700; text-transform:uppercase; }
        .perfil-meta__value { color:#2a22a6; font-size:1rem; font-weight:700; }
        .perfil-form label { color:#2a22a6; font-weight:700; font-size:.88rem; margin-bottom:.3rem; }
        .perfil-form .form-control { min-height:44px; border:1px solid #cfe2f3; border-radius:12px; box-shadow:none; }
        .perfil-form .form-control:focus { border-color:#62afe2; box-shadow:0 0 0 .16rem rgba(98,175,226,.18); }
        .perfil-btn { min-height: 46px; border:0; border-radius:14px; font-weight:700; background:linear-gradient(135deg,#3c35c6 0%,#2a22a6 100%); box-shadow:0 12px 24px rgba(42,34,166,.18); }
        @media (max-width: 991.98px) { .perfil-grid { grid-template-columns: 1fr; } }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="perfil-page">
        <section class="container-fluid">
            <div class="perfil-header">
                <h1 class="perfil-header__title"><i class="far fa-id-badge"></i> Perfil</h1>
            </div>
        </section>
        <div class="perfil-divider"></div>

        <div class="perfil-grid">
            <section class="perfil-card">
                <div class="perfil-card__title">Datos del administrador</div>
                <p class="perfil-card__text">Resumen del usuario autenticado y su informacion de seguridad registrada.</p>
                <div class="perfil-meta">
                    <div class="perfil-meta__item">
                        <span class="perfil-meta__label">Nombre</span>
                        <asp:Label runat="server" ID="lblNombreUsuario" CssClass="perfil-meta__value" />
                    </div>
                    <div class="perfil-meta__item">
                        <span class="perfil-meta__label">Usuario</span>
                        <asp:Label runat="server" ID="lblLoginUsuario" CssClass="perfil-meta__value" />
                    </div>
                    <div class="perfil-meta__item">
                        <span class="perfil-meta__label">WhatsApp de Recuperacion</span>
                        <asp:Label runat="server" ID="lblWhatsAppUsuario" CssClass="perfil-meta__value" />
                    </div>
                </div>
            </section>

            <section class="perfil-card">
                <div class="perfil-card__title">Cambiar clave</div>
                <p class="perfil-card__text">Actualiza tu clave desde el panel usando tu clave actual.</p>
                <div class="perfil-form">
                    <div class="form-group">
                        <label>Clave Actual</label>
                        <asp:TextBox runat="server" ID="txtClaveActual" TextMode="Password" CssClass="form-control" />
                    </div>
                    <div class="form-group">
                        <label>Nueva Clave</label>
                        <asp:TextBox runat="server" ID="txtClavePerfilNueva" TextMode="Password" CssClass="form-control" />
                    </div>
                    <div class="form-group">
                        <label>Confirmar Nueva Clave</label>
                        <asp:TextBox runat="server" ID="txtClavePerfilNueva2" TextMode="Password" CssClass="form-control" />
                    </div>
                    <asp:Button runat="server" ID="btnCambiarClave" Text="Actualizar Clave" CssClass="btn btn-primary btn-block perfil-btn" OnClick="btnCambiarClave_Click" />
                </div>
            </section>
        </div>
    </div>
</asp:Content>

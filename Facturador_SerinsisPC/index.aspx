<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="Facturador_SerinsisPC.index" Async="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .cuentas-page {
            display: flex;
            flex-direction: column;
            gap: .9rem;
        }

        .cuentas-header {
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: 1rem;
            margin-bottom: .1rem;
        }

        .cuentas-header__title {
            display: flex;
            align-items: center;
            gap: .7rem;
            color: #2a22a6;
            margin: 0;
            font-size: 2rem;
            font-weight: 700;
        }

        .cuentas-header__title i {
            color: #62afe2;
            font-size: 1.7rem;
        }

        .cuentas-divider {
            height: 1px;
            background: linear-gradient(90deg, #c9e3f5 0%, rgba(201, 227, 245, 0) 100%);
            margin: 0;
        }

        .cuentas-panel {
            background: linear-gradient(135deg, #fbfdff 0%, #f3f9ff 100%);
            border: 1px solid #cfe2f3;
            border-radius: 16px;
            padding: .95rem 1.05rem;
            box-shadow: 0 10px 24px rgba(42, 34, 166, 0.05);
        }

        .cuentas-panel__title {
            color: #2a22a6;
            font-size: .96rem;
            font-weight: 700;
            margin-bottom: .3rem;
        }

        .cuentas-panel__text {
            color: #61779c;
            margin-bottom: 0;
            font-size: .94rem;
        }

        .cuentas-legend {
            display: flex;
            flex-wrap: wrap;
            gap: .6rem;
            margin-top: .8rem;
        }

        .cuentas-legend__item {
            display: inline-flex;
            align-items: center;
            gap: .45rem;
            background: #ffffff;
            border: 1px solid #d7e6f4;
            border-radius: 999px;
            padding: .45rem .8rem;
            color: #2c3e68;
            font-size: .88rem;
            font-weight: 600;
        }

        .cuentas-action {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            width: 36px;
            height: 36px;
            border-radius: 11px;
            margin: 0 .08rem;
            transition: transform .15s ease, box-shadow .15s ease;
        }

        .cuentas-action:hover {
            transform: translateY(-1px);
            text-decoration: none;
            box-shadow: 0 8px 18px rgba(0, 0, 0, 0.12);
        }

        .cuentas-action--aviso {
            background: #edf3ff;
            color: #2a22a6;
        }

        .cuentas-action--suspender {
            background: #fff0f2;
            color: #d8344a;
        }

        .cuentas-action--reconectar {
            background: #ecfbf2;
            color: #1f9b53;
        }

        .cuentas-toolbar {
            display: flex;
            flex-wrap: wrap;
            gap: .8rem;
            margin-bottom: .65rem;
        }

        .cuentas-toolbar__group {
            flex: 1 1 420px;
            background: #ffffff;
            border: 1px solid #d6e4f3;
            border-radius: 15px;
            padding: .8rem .9rem;
            box-shadow: 0 8px 18px rgba(42, 34, 166, 0.04);
        }

        .cuentas-toolbar__title {
            color: #2a22a6;
            font-size: .88rem;
            font-weight: 700;
            margin-bottom: .55rem;
        }

        .cuentas-toolbar__actions {
            display: flex;
            flex-wrap: wrap;
            gap: .5rem;
        }

        .cuentas-mass-btn {
            border-radius: 10px;
            font-weight: 600;
            padding: .38rem .75rem;
            font-size: .9rem;
        }

        .cuentas-select {
            width: 16px;
            height: 16px;
            accent-color: #2a22a6;
            cursor: pointer;
        }

        .cuentas-page .container-fluid {
            padding-left: .2rem;
            padding-right: .2rem;
        }

        .carga-overlay {
            position: fixed;
            inset: 0;
            background: rgba(255, 255, 255, 0.82);
            backdrop-filter: blur(2px);
            z-index: 9999;
            display: none;
            align-items: center;
            justify-content: center;
        }

        .carga-overlay__content {
            min-width: 220px;
            text-align: center;
            background: #ffffff;
            border: 1px solid #d7e5f3;
            border-radius: 16px;
            padding: 1.2rem 1.5rem;
            box-shadow: 0 18px 40px rgba(42, 34, 166, 0.10);
        }

        .carga-overlay__spinner {
            width: 58px;
            height: 58px;
            margin: 0 auto .9rem;
            border-radius: 50%;
            border: 5px solid #dcebf8;
            border-top-color: #2a22a6;
            animation: giroCarga .8s linear infinite;
        }

        .carga-overlay__title {
            color: #2a22a6;
            font-size: .96rem;
            font-weight: 700;
            margin-bottom: .25rem;
        }

        .carga-overlay__text {
            color: #61779c;
            font-size: .88rem;
            margin-bottom: 0;
        }

        @keyframes giroCarga {
            from {
                transform: rotate(0deg);
            }

            to {
                transform: rotate(360deg);
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="overlayCarga" class="carga-overlay">
        <div class="carga-overlay__content">
            <div class="carga-overlay__spinner"></div>
            <div class="carga-overlay__title">Procesando solicitud</div>
            <p class="carga-overlay__text">Estamos actualizando el estado de las cuentas seleccionadas.</p>
        </div>
    </div>

    <div class="cuentas-page">
    <section class="container-fluid">
        <div class="cuentas-header">
            <h1 class="cuentas-header__title"><i class="fas fa-file-invoice"></i> Cuentas</h1>
        </div>
        <div class="cuentas-divider"></div>
    </section>

    <section class="container-fluid">
        <div class="cuentas-panel">
            <div class="cuentas-panel__title">Gestión de estado de cuentas</div>
            <p class="cuentas-panel__text">
                Usa esta pantalla para aplicar aviso preventivo, suspensión o reconexión del servicio. Cuando un cliente tiene varias bases de datos asociadas, el cambio se refleja en todas.
            </p>
            <div class="cuentas-legend">
                <span class="cuentas-legend__item"><i class="fas fa-cut text-primary"></i> Aviso preventivo: estado 1</span>
                <span class="cuentas-legend__item"><i class="fas fa-cut text-danger"></i> Suspensión: estado 2</span>
                <span class="cuentas-legend__item"><i class="fas fa-plug text-success"></i> Reconexión: estado 0</span>
            </div>
        </div>
    </section>

    <section class="container-fluid">
        <div class="cuentas-toolbar">
            <div class="cuentas-toolbar__group">
                <div class="cuentas-toolbar__title">Acciones masivas sobre todos los clientes listados</div>
                <div class="cuentas-toolbar__actions">
                    <asp:LinkButton runat="server" ID="btnAvisoTodos" CssClass="btn btn-outline-primary cuentas-mass-btn" OnClientClick="mostrarCarga();" OnClick="btnAvisoTodos_Click"><i class="fas fa-cut"></i>&nbsp;Aviso a todos</asp:LinkButton>
                    <asp:LinkButton runat="server" ID="btnSustemderTodo" CssClass="btn btn-outline-danger cuentas-mass-btn" OnClientClick="mostrarCarga();" OnClick="btnSustemderTodo_Click"><i class="fas fa-ban"></i>&nbsp;Suspender a todos</asp:LinkButton>
                    <asp:LinkButton runat="server" ID="btnReconectarTodos" CssClass="btn btn-outline-success cuentas-mass-btn" OnClientClick="mostrarCarga();" OnClick="btnReconectarTodos_Click"><i class="fas fa-plug"></i>&nbsp;Reconectar a todos</asp:LinkButton>
                </div>
            </div>
            <div class="cuentas-toolbar__group">
                <div class="cuentas-toolbar__title">Acciones sobre clientes seleccionados</div>
                <div class="cuentas-toolbar__actions">
                    <asp:LinkButton runat="server" ID="btnAvisoSeleccionados" CssClass="btn btn-primary cuentas-mass-btn" OnClientClick="mostrarCarga();" OnClick="btnAvisoSeleccionados_Click"><i class="fas fa-cut"></i>&nbsp;Aviso seleccionados</asp:LinkButton>
                    <asp:LinkButton runat="server" ID="btnSuspenderSeleccionados" CssClass="btn btn-danger cuentas-mass-btn" OnClientClick="mostrarCarga();" OnClick="btnSuspenderSeleccionados_Click"><i class="fas fa-cut"></i>&nbsp;Suspender seleccionados</asp:LinkButton>
                    <asp:LinkButton runat="server" ID="btnReconectarSeleccionados" CssClass="btn btn-success cuentas-mass-btn" OnClientClick="mostrarCarga();" OnClick="btnReconectarSeleccionados_Click"><i class="fas fa-plug"></i>&nbsp;Reconectar seleccionados</asp:LinkButton>
                </div>
            </div>
        </div>
    </section>

    <%-- Lista Facturas --%>
    <section class="table-container cuentas-panel" style="padding:.2rem; width:100%;">
        <table id="tablaCuentas" class="table-cebra">
            <thead>
                <tr>
                    <th style="min-width: 40px;" class="sticky text-center">
                        <input type="checkbox" id="chkSeleccionarTodos" class="cuentas-select" onclick="toggleSeleccionClientes(this)" title="Seleccionar todos" />
                    </th>
                    <th style="min-width: 15px;">#</th>
                    <th>Cliente</th>
                    <th>Comercio</th>
                    <th>WharsApp</th>
                    <th>Saldo</th>
                    <th>Estado</th>
                    <th>Opciones</th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater runat="server" ID="rpCuentas">
                    <ItemTemplate>
                        <tr>
                            <td class="sticky text-center">
                                <asp:CheckBox runat="server" ID="chkSeleccionar" CssClass="cuentas-select cuentas-select-item" />
                                <asp:HiddenField runat="server" ID="hfIdCliente" Value='<%# Eval("id") %>' />
                            </td>
                            <td style="min-width:15px;"><%# Container.ItemIndex +1 %></td>
                            <td><%# Eval("nombreRepresentate") %></td>
                            <td><%# Eval("nombreComercial") %></td>
                            <td><%# Eval("celular") %></td>
                            <td><%# $"{Eval("total"):C0}" %></td>
                            <td><%# Eval("estado") %></td>
                            <td class="text-center">
                                <asp:LinkButton runat="server" ID="btnAbiso" CssClass="cuentas-action cuentas-action--aviso" ToolTip="Marcar en aviso preventivo (estado 1)" OnClientClick="mostrarCarga();" OnClick="btnAbiso_Click" CommandArgument='<%# Eval("id") %>'><i class="fas fa-cut"></i></asp:LinkButton>
                                <asp:LinkButton runat="server" ID="btnSuspender" CssClass="cuentas-action cuentas-action--suspender" ToolTip="Suspender servicio (estado 2)" OnClientClick="mostrarCarga();" OnClick="btnSuspender_Click" CommandArgument='<%# Eval("id") %>'><i class="fas fa-cut"></i></asp:LinkButton>
                                <asp:LinkButton runat="server" ID="btnConectar" CssClass="cuentas-action cuentas-action--reconectar" ToolTip="Reconectar servicio (estado 0)" OnClientClick="mostrarCarga();" OnClick="btnConectar_Click" CommandArgument='<%# Eval("id") %>'><i class="fas fa-plug"></i></asp:LinkButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </section>
    </div>

    
<script src="js/myScripts.js"></script>
<script>
    function mostrarCarga() {
        var overlay = document.getElementById('overlayCarga');
        if (overlay) {
            overlay.style.display = 'flex';
        }
    }

    function toggleSeleccionClientes(source) {
        var checkboxes = document.querySelectorAll('.cuentas-select-item input[type="checkbox"], input.cuentas-select-item[type="checkbox"]');
        for (var i = 0; i < checkboxes.length; i++) {
            checkboxes[i].checked = source.checked;
        }
    }
</script>

</asp:Content>



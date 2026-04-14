<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="facturar.aspx.cs" Inherits="Facturador_SerinsisPC.facturar" Async="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .facturar-page { display: flex; flex-direction: column; gap: .9rem; }
        .facturar-page .container-fluid { padding-left: .2rem; padding-right: .2rem; }
        .facturar-header { display: flex; align-items: center; justify-content: space-between; gap: 1rem; }
        .facturar-header__title { display: flex; align-items: center; gap: .7rem; color: #2a22a6; margin: 0; font-size: 2rem; font-weight: 700; }
        .facturar-header__title i { color: #62afe2; font-size: 1.7rem; }
        .facturar-summary { display: inline-flex; flex-direction: column; align-items: flex-end; background: linear-gradient(135deg, #ffffff 0%, #eef8fe 100%); border: 1px solid #d5e5f4; border-radius: 14px; padding: .55rem .85rem; min-width: 160px; }
        .facturar-summary__label { color: #61779c; font-size: .78rem; font-weight: 700; text-transform: uppercase; letter-spacing: .05em; }
        .facturar-summary__value { color: #2a22a6; font-size: 1.25rem; font-weight: 700; }
        .facturar-divider { height: 1px; background: linear-gradient(90deg, #c9e3f5 0%, rgba(201, 227, 245, 0) 100%); }
        .facturar-card, .facturar-table-wrap { background: linear-gradient(135deg, #fbfdff 0%, #f3f9ff 100%); border: 1px solid #cfe2f3; border-radius: 16px; box-shadow: 0 10px 24px rgba(42, 34, 166, 0.05); }
        .facturar-card { padding: .95rem 1rem; }
        .facturar-table-wrap { padding: .3rem; }
        .facturar-card__title { color: #2a22a6; font-weight: 700; font-size: .95rem; margin-bottom: .25rem; }
        .facturar-card__text { color: #61779c; font-size: .92rem; margin-bottom: .8rem; }
        .facturar-form label { color: #2a22a6; font-weight: 700; font-size: .88rem; margin-bottom: .3rem; }
        .facturar-form .form-control { min-height: 42px; border: 1px solid #cfe2f3; border-radius: 12px; color: #243a60; box-shadow: none; }
        .facturar-form .form-control:focus { border-color: #62afe2; box-shadow: 0 0 0 .16rem rgba(98, 175, 226, 0.18); }
        .facturar-readonly { background: #f3f8ff; }
        .facturar-search-btn, .facturar-main-btn { border-radius: 12px; font-weight: 700; }
        .facturar-search-btn { background: #eef8fe; border: 1px solid #cfe2f3; color: #2a22a6; }
        .facturar-main-btn { min-width: 140px; padding: .6rem .95rem; background: linear-gradient(135deg, #3c35c6 0%, #2a22a6 100%); border: 0; box-shadow: 0 12px 22px rgba(42, 34, 166, 0.16); }
        .facturar-state { display: inline-flex; align-items: center; gap: .45rem; padding: .35rem .6rem; border-radius: 999px; font-size: .84rem; font-weight: 700; }
        .facturar-state--pendiente { background: #fff7e7; color: #c98916; }
        .facturar-state--pagada { background: #ecfbf2; color: #1f9b53; }
        .facturar-state--otro { background: #eef8fe; color: #2a22a6; }
        .facturar-option { display: inline-flex; align-items: center; justify-content: center; width: 36px; height: 36px; border-radius: 10px; text-decoration: none; transition: transform .15s ease, box-shadow .15s ease; }
        .facturar-option:hover { text-decoration: none; transform: translateY(-1px); box-shadow: 0 8px 16px rgba(42, 34, 166, 0.10); }
        .facturar-option--pay { background: #ecfbf2; color: #1f9b53; }
        .facturar-option--delete { background: #fff0f2; color: #d8344a; }
        .facturar-modal .modal-content { border: 1px solid #cfe2f3; border-radius: 18px; box-shadow: 0 22px 46px rgba(42, 34, 166, 0.14); overflow: hidden; }
        .facturar-page .Modal-padre { display: flex; align-items: center; justify-content: center; padding: 1.25rem; background: rgba(16, 27, 55, 0.28); backdrop-filter: blur(3px); z-index: 1200; }
        .facturar-modal.modal-dialog { width: 100%; max-width: 980px !important; margin: 0 auto; }
        .facturar-modal .modal-header { background: linear-gradient(135deg, #2a22a6 0%, #62afe2 100%); color: #ffffff; border-bottom: 0; padding: .9rem 1rem; display: flex; align-items: center; justify-content: space-between; }
        .facturar-modal .modal-title { font-weight: 700; margin: 0; }
        .facturar-modal .close, .facturar-modal .close a { color: #ffffff; opacity: 1; text-decoration: none; }
        .facturar-modal .modal-body { background: #fbfdff; padding: 1rem; }
        .facturar-modal .modal-footer { background: #f5f9ff; border-top: 1px solid #d8e6f4; padding: .8rem 1rem; display: flex; justify-content: flex-end; gap: .6rem; }
        .facturar-select-btn { display: inline-flex; align-items: center; justify-content: center; width: 34px; height: 34px; border-radius: 10px; background: #eef8fe; color: #2a22a6; text-decoration: none; }
        .facturar-meta { display: grid; grid-template-columns: repeat(3, minmax(0, 1fr)); gap: .75rem; margin-top: .8rem; }
        .facturar-meta__item { background: #ffffff; border: 1px solid #d8e6f4; border-radius: 14px; padding: .7rem .8rem; }
        .facturar-meta__label { display: block; color: #61779c; font-size: .78rem; font-weight: 700; text-transform: uppercase; }
        .facturar-meta__value { color: #2a22a6; font-size: 1rem; font-weight: 700; }
        .facturar-table-toolbar { display: flex; justify-content: flex-end; margin: .2rem .35rem .55rem; }
        .facturar-table-filter { display: flex; align-items: center; gap: .55rem; }
        .facturar-table-filter label { margin: 0; color: #2a22a6; font-weight: 700; font-size: .88rem; }
        .facturar-table-filter select { min-width: 150px; min-height: 40px; border: 1px solid #cfe2f3; border-radius: 12px; color: #243a60; box-shadow: none; background: #ffffff; padding: .4rem .75rem; }
        @media (max-width: 767.98px) {
            .facturar-page .Modal-padre { padding: .55rem; align-items: flex-start; overflow-y: auto; }
            .facturar-modal.modal-dialog { max-width: 100% !important; }
            .facturar-meta { grid-template-columns: 1fr; }
            .facturar-table-toolbar { justify-content: stretch; }
            .facturar-table-filter { width: 100%; flex-direction: column; align-items: stretch; }
            .facturar-table-filter select { width: 100%; }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="facturar-page">
        <asp:Panel ID="panelModalBuscarCliente" Visible="false" runat="server" CssClass="Modal-padre">
            <div tabindex="-1" role="dialog" aria-hidden="true">
                <div class="modal-dialog modal-lg facturar-modal" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title"><asp:Label Text="Buscar Cliente" runat="server" ID="lblTituloModal" /></h5>
                            <button type="button" class="close p-0 m-0 border-0 bg-transparent" aria-label="Close">
                                <asp:LinkButton ID="btnCerrarModalBuscarCliente" runat="server" OnClick="btnCerrarModalBuscarCliente_Click"><i class="fas fa-times"></i></asp:LinkButton>
                            </button>
                        </div>
                        <div class="modal-body" style="overflow-y: auto">
                            <section class="facturar-table-wrap">
                                <table id="tablaClientes" class="table-cebra" style="width:100%;">
                                    <thead>
                                        <tr>
                                            <th>#</th>
                                            <th>NIT</th>
                                            <th>Razon Social</th>
                                            <th>Representante</th>
                                            <th>WhatsApp</th>
                                            <th>Seleccionar</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater runat="server" ID="rpClientes">
                                            <ItemTemplate>
                                                <tr>
                                                    <td><%# Container.ItemIndex +1 %></td>
                                                    <td><%# Eval("nit") %></td>
                                                    <td><%# Eval("nombreComercial") %></td>
                                                    <td><%# Eval("nombreRepresentate") %></td>
                                                    <td><%# Eval("celular") %></td>
                                                    <td>
                                                        <asp:LinkButton runat="server" ID="btnSeleccionarCliente" CssClass="facturar-select-btn" OnClick="btnSeleccionarCliente_Click" CausesValidation="false" CommandArgument='<%# Eval("id") %>'><i class="fas fa-check"></i></asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>
                            </section>
                        </div>
                        <div class="modal-footer">
                            <asp:LinkButton ID="btnCerrarModalBuscarCliente2" runat="server" class="btn btn-secondary" OnClick="btnCerrarModalBuscarCliente2_Click">Cerrar</asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>

        <asp:Panel ID="panelModalPago" Visible="false" runat="server" CssClass="Modal-padre">
            <div tabindex="-1" role="dialog" aria-hidden="true">
                <div class="modal-dialog modal-lg facturar-modal" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title"><i class="fas fa-wallet"></i>&nbsp;Registrar Pago</h5>
                            <button type="button" class="close p-0 m-0 border-0 bg-transparent" aria-label="Close">
                                <asp:LinkButton ID="btnCerrarModalPago1" runat="server" OnClick="btnCerrarModalPago_Click"><i class="fas fa-times"></i></asp:LinkButton>
                            </button>
                        </div>
                        <div class="modal-body">
                            <div class="facturar-card__title">Pago para: <asp:Label runat="server" ID="lblPagoFactura" /></div>
                            <p class="facturar-card__text">Registra el ingreso real recibido y deja trazabilidad del comprobante y el metodo de pago.</p>
                            <div class="container-fluid facturar-form">
                                <div class="row">
                                    <div class="col-md-4">
                                        <label>Fecha de Pago</label>
                                        <asp:TextBox runat="server" ID="txtFechaPago" CssClass="form-control" TextMode="Date" />
                                    </div>
                                    <div class="col-md-4">
                                        <label>Metodo de Pago</label>
                                        <asp:DropDownList runat="server" ID="ddlMetodoPago" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                    <div class="col-md-4">
                                        <label>Valor Recibido</label>
                                        <asp:TextBox runat="server" ID="txtValorPago" CssClass="form-control" TextMode="Number" />
                                    </div>
                                    <div class="col-md-6">
                                        <label>Numero de Comprobante</label>
                                        <asp:TextBox runat="server" ID="txtNumeroComprobante" CssClass="form-control" />
                                    </div>
                                    <div class="col-md-6">
                                        <label>Referencia</label>
                                        <asp:TextBox runat="server" ID="txtReferenciaPago" CssClass="form-control" />
                                    </div>
                                    <div class="col-md-12">
                                        <label>Observacion</label>
                                        <asp:TextBox runat="server" ID="txtObservacionPago" CssClass="form-control" TextMode="MultiLine" Rows="3" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:LinkButton ID="btnCerrarModalPago2" runat="server" class="btn btn-secondary" OnClick="btnCerrarModalPago_Click">Cerrar</asp:LinkButton>
                            <asp:Button Text="Registrar Pago" runat="server" ID="btnRegistrarPago" CssClass="btn btn-primary facturar-main-btn" OnClick="btnRegistrarPago_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>

        <section class="container-fluid">
            <div class="facturar-header">
                <h1 class="facturar-header__title"><i class="fas fa-file-invoice-dollar"></i> Facturacion y Cartera</h1>
                <div class="facturar-summary">
                    <span class="facturar-summary__label">Pendiente acumulado</span>
                    <span class="facturar-summary__value"><%= $"{ViewState["ValorPendiente"]:C0}" %></span>
                </div>
            </div>
        </section>
        <div class="facturar-divider"></div>

        <section class="facturar-card">
            <div class="facturar-card__title">Datos de facturacion</div>
            <p class="facturar-card__text">Genera la factura con fecha de vencimiento, control de periodo y saldo inicial pendiente.</p>

            <div class="container-fluid facturar-form">
                <div class="row">
                    <div class="col-md-2">
                        <label>Anio</label>
                        <asp:TextBox runat="server" ID="txtyear" CssClass="form-control" TextMode="Number" />
                    </div>
                    <div class="col-md-2">
                        <label>Mes</label>
                        <asp:DropDownList runat="server" ID="ddl_Mes" CssClass="form-control" OnSelectedIndexChanged="ddl_Mes_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                    <div class="form-group col-md-8">
                        <label>Cliente</label>
                        <div class="input-group">
                            <asp:TextBox runat="server" ID="txtCliente" CssClass="form-control facturar-readonly" ReadOnly="true" />
                            <div class="input-group-append">
                                <asp:LinkButton runat="server" class="btn facturar-search-btn" ID="btnBUscarCliente" OnClick="btnBUscarCliente_Click" CausesValidation="false"><i class="fas fa-search"></i>&nbsp;Buscar</asp:LinkButton>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <label>Valor Plan</label>
                        <asp:TextBox runat="server" ID="txtValorPlan" CssClass="form-control facturar-readonly" TextMode="Number" ReadOnly="true" />
                    </div>
                    <div class="col-md-2">
                        <label>Sedes</label>
                        <asp:TextBox runat="server" ID="txtSedes" CssClass="form-control facturar-readonly" TextMode="Number" ReadOnly="true" />
                    </div>
                    <div class="col-md-2">
                        <label>Valor a Pagar</label>
                        <asp:TextBox runat="server" ID="txtValorAPagar" CssClass="form-control" TextMode="Number" />
                    </div>
                    <div class="col-md-3">
                        <label>Dia de Pago</label>
                        <asp:TextBox runat="server" ID="txtDiaPagoCliente" CssClass="form-control facturar-readonly" TextMode="Number" ReadOnly="true" />
                    </div>
                    <div class="col-md-3">
                        <label>Proximo Pago</label>
                        <asp:TextBox runat="server" ID="txtProximoPagoCliente" CssClass="form-control facturar-readonly" TextMode="Date" ReadOnly="true" />
                    </div>
                    <div class="col-md-3">
                        <label>Fecha Vencimiento</label>
                        <asp:TextBox runat="server" ID="txtFechaVencimientoFactura" CssClass="form-control" TextMode="Date" />
                    </div>
                    <div class="col-md-2 d-flex align-items-end">
                        <asp:Button Text="Facturar" runat="server" ID="btnFacturar" CssClass="btn btn-primary facturar-main-btn w-100" OnClick="btnFacturar_Click" />
                    </div>
                </div>
                <div class="facturar-meta">
                    <div class="facturar-meta__item">
                        <span class="facturar-meta__label">Periodicidad</span>
                        <asp:Label runat="server" ID="lblPeriodicidadCliente" CssClass="facturar-meta__value" Text="Sin definir" />
                    </div>
                    <div class="facturar-meta__item">
                        <span class="facturar-meta__label">Inicio del Plan</span>
                        <asp:Label runat="server" ID="lblInicioPlanCliente" CssClass="facturar-meta__value" Text="Sin definir" />
                    </div>
                    <div class="facturar-meta__item">
                        <span class="facturar-meta__label">Ultimo Pago</span>
                        <asp:Label runat="server" ID="lblUltimoPagoCliente" CssClass="facturar-meta__value" Text="Sin registrar" />
                    </div>
                </div>
            </div>
        </section>

        <section class="facturar-table-wrap">
            <div class="facturar-table-toolbar">
                <div class="facturar-table-filter">
                    <label for="<%= ddlFiltroEstadoFacturas.ClientID %>">Estado</label>
                    <asp:DropDownList runat="server" ID="ddlFiltroEstadoFacturas" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltroEstadoFacturas_SelectedIndexChanged" />
                </div>
            </div>
            <table id="tablaFacturas" class="table-cebra">
                <thead>
                    <tr>
                        <th>#</th>
                        <th>Fecha</th>
                        <th>Cliente</th>
                        <th>Comercio</th>
                        <th>Anio</th>
                        <th>Mes</th>
                        <th>Vence</th>
                        <th>Total</th>
                        <th>Saldo</th>
                        <th>Estado</th>
                        <th>Pagar</th>
                        <th>Eliminar</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater runat="server" ID="rpFacturas">
                        <ItemTemplate>
                            <tr>
                                <td><%# Container.ItemIndex +1 %></td>
                                <td><%# Eval("fechaFactura", "{0:yyyy-MM-dd}") %></td>
                                <td><%# Eval("nombreRepresentate") %></td>
                                <td><%# Eval("nombreComercial") %></td>
                                <td><%# Eval("yearFactura") %></td>
                                <td><%# Eval("nombreMes") %></td>
                                <td><%# Eval("fechaVencimiento", "{0:yyyy-MM-dd}") %></td>
                                <td><%# $"{Eval("valorAPagar"):C0}"  %></td>
                                <td><%# $"{Eval("saldoPendiente"):C0}"  %></td>
                                <td>
                                    <span class='facturar-state <%# Eval("nombreEstado") != null && Eval("nombreEstado").ToString().ToLower().Contains("pag") ? "facturar-state--pagada" : Eval("nombreEstado") != null && Eval("nombreEstado").ToString().ToLower().Contains("pend") ? "facturar-state--pendiente" : "facturar-state--otro" %>'>
                                        <%# Eval("nombreEstado") %>
                                    </span>
                                </td>
                                <td class="text-center">
                                    <asp:LinkButton runat="server" ID="btnPagar" CssClass="facturar-option facturar-option--pay" OnClick="btnPagar_Click" CausesValidation="false" CommandArgument='<%# Eval("id") %>'><i class="fas fa-dollar-sign"></i></asp:LinkButton>
                                </td>
                                <td class="text-center">
                                    <asp:LinkButton runat="server" ID="btnEliminar" CssClass="facturar-option facturar-option--delete" OnClick="btnEliminar_Click" CausesValidation="false" CommandArgument='<%# Eval("id") %>'><i class="fas fa-trash-alt"></i></asp:LinkButton>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </section>
    </div>

    <script src="js/myScripts.js"></script>
</asp:Content>

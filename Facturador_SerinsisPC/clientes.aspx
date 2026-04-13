<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="clientes.aspx.cs" Inherits="Facturador_SerinsisPC.clientes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .clientes-page {
            display: flex;
            flex-direction: column;
            gap: .9rem;
        }

        .clientes-page .container-fluid {
            padding-left: .2rem;
            padding-right: .2rem;
        }

        .clientes-header {
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: 1rem;
        }

        .clientes-header__title {
            display: flex;
            align-items: center;
            gap: .7rem;
            color: #2a22a6;
            margin: 0;
            font-size: 2rem;
            font-weight: 700;
        }

        .clientes-header__title i {
            color: #62afe2;
            font-size: 1.7rem;
        }

        .clientes-divider {
            height: 1px;
            background: linear-gradient(90deg, #c9e3f5 0%, rgba(201, 227, 245, 0) 100%);
        }

        .clientes-primary-btn {
            border-radius: 12px;
            padding: .6rem 1rem;
            font-weight: 700;
            background: linear-gradient(135deg, #3c35c6 0%, #2a22a6 100%);
            border: 0;
            box-shadow: 0 12px 22px rgba(42, 34, 166, 0.16);
        }

        .clientes-primary-btn:hover {
            transform: translateY(-1px);
            box-shadow: 0 14px 24px rgba(42, 34, 166, 0.18);
        }

        .clientes-table-wrap {
            background: linear-gradient(135deg, #fbfdff 0%, #f3f9ff 100%);
            border: 1px solid #cfe2f3;
            border-radius: 16px;
            padding: .3rem;
            box-shadow: 0 10px 24px rgba(42, 34, 166, 0.05);
        }

        .clientes-table {
            width: 100%;
        }

        .clientes-table thead th {
            white-space: nowrap;
        }

        .clientes-state {
            display: inline-flex;
            align-items: center;
            gap: .45rem;
            font-weight: 700;
            padding: .35rem .6rem;
            border-radius: 999px;
            font-size: .84rem;
            margin-right: .35rem;
        }

        .clientes-state--activo {
            background: #ecfbf2;
            color: #1f9b53;
        }

        .clientes-state--inactivo {
            background: #fff0f2;
            color: #d8344a;
        }

        .clientes-option {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            width: 34px;
            height: 34px;
            border-radius: 10px;
            margin-right: .2rem;
            text-decoration: none;
            transition: transform .15s ease, box-shadow .15s ease;
        }

        .clientes-option:hover {
            text-decoration: none;
            transform: translateY(-1px);
            box-shadow: 0 8px 16px rgba(42, 34, 166, 0.10);
        }

        .clientes-option--db {
            background: #eef8fe;
            color: #2a22a6;
        }

        .clientes-option--edit {
            background: #edf3ff;
            color: #2a22a6;
        }

        .clientes-option--delete {
            background: #fff0f2;
            color: #d8344a;
        }

        .clientes-option--toggle {
            background: #eef8fe;
            color: #2a22a6;
        }

        .clientes-modal .modal-content {
            border: 1px solid #cfe2f3;
            border-radius: 18px;
            box-shadow: 0 22px 46px rgba(42, 34, 166, 0.14);
            overflow: hidden;
        }

        .clientes-page .Modal-padre {
            display: flex;
            align-items: center;
            justify-content: center;
            padding: 1.25rem;
            background: rgba(16, 27, 55, 0.28);
            backdrop-filter: blur(3px);
            z-index: 1200;
        }

        .clientes-modal.modal-dialog {
            width: 100%;
            max-width: 980px !important;
            margin: 0 auto;
        }

        .clientes-modal .modal-header {
            background: linear-gradient(135deg, #2a22a6 0%, #62afe2 100%);
            color: #ffffff;
            border-bottom: 0;
            padding: .9rem 1rem;
            display: flex;
            align-items: center;
            justify-content: space-between;
        }

        .clientes-modal .modal-title {
            font-weight: 700;
            margin: 0;
        }

        .clientes-modal .close,
        .clientes-modal .close a {
            color: #ffffff;
            opacity: 1;
            text-decoration: none;
        }

        .clientes-modal .modal-body {
            background: #fbfdff;
            padding: 1rem;
        }

        .clientes-modal label {
            color: #2a22a6;
            font-weight: 700;
            font-size: .88rem;
            margin-bottom: .3rem;
        }

        .clientes-modal .form-control {
            border: 1px solid #cfe2f3;
            border-radius: 12px;
            min-height: 42px;
            color: #243a60;
            box-shadow: none;
        }

        .clientes-modal .form-control:focus {
            border-color: #62afe2;
            box-shadow: 0 0 0 .16rem rgba(98, 175, 226, 0.18);
        }

        .clientes-modal .modal-footer {
            background: #f5f9ff;
            border-top: 1px solid #d8e6f4;
            padding: .8rem 1rem;
            display: flex;
            justify-content: flex-end;
            gap: .6rem;
        }

        .clientes-modal .btn {
            border-radius: 12px;
            font-weight: 700;
            padding: .5rem .95rem;
        }

        .db-client-chip {
            display: inline-flex;
            align-items: center;
            gap: .45rem;
            background: #eef8fe;
            color: #2a22a6;
            border: 1px solid #d7e5f3;
            border-radius: 999px;
            padding: .4rem .75rem;
            font-weight: 700;
            font-size: .86rem;
            margin-bottom: .85rem;
        }

        .db-inline-card {
            background: #ffffff;
            border: 1px solid #d7e5f3;
            border-radius: 16px;
            padding: .95rem 1rem;
            margin-bottom: .9rem;
        }

        .db-inline-grid {
            display: grid;
            grid-template-columns: minmax(190px, .8fr) minmax(260px, 1.3fr) auto;
            gap: .8rem;
            align-items: end;
        }

        .db-inline-title {
            color: #2a22a6;
            font-weight: 700;
            font-size: .9rem;
            margin-bottom: .25rem;
        }

        .db-inline-text {
            color: #61779c;
            font-size: .9rem;
            margin-bottom: .75rem;
        }

        .db-inline-grid label {
            color: #2a22a6;
            font-weight: 700;
            font-size: .88rem;
            margin-bottom: .35rem;
            display: block;
        }

        .db-inline-grid .form-control {
            min-height: 42px;
            border: 1px solid #cfe2f3;
            border-radius: 12px;
            color: #243a60;
            box-shadow: none;
        }

        .db-inline-grid .form-control:focus {
            border-color: #62afe2;
            box-shadow: 0 0 0 .16rem rgba(98, 175, 226, 0.18);
        }

        .db-inline-btn {
            border-radius: 12px;
            padding: .6rem 1rem;
            font-weight: 700;
            background: linear-gradient(135deg, #3c35c6 0%, #2a22a6 100%);
            border: 0;
            box-shadow: 0 12px 22px rgba(42, 34, 166, 0.16);
        }

        .db-inline-table {
            background: #ffffff;
            border: 1px solid #d7e5f3;
            border-radius: 16px;
            padding: .3rem;
        }

        .db-inline-state {
            display: inline-flex;
            align-items: center;
            gap: .45rem;
            font-weight: 700;
            padding: .35rem .6rem;
            border-radius: 999px;
            font-size: .84rem;
        }

        .db-inline-state--activo {
            background: #ecfbf2;
            color: #1f9b53;
        }

        .db-inline-state--inactivo {
            background: #fff0f2;
            color: #d8344a;
        }

        .db-inline-option {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            width: 34px;
            height: 34px;
            border-radius: 10px;
            text-decoration: none;
            background: #fff0f2;
            color: #d8344a;
        }

        @media (max-width: 991.98px) {
            .db-inline-grid {
                grid-template-columns: 1fr;
            }
        }

        .clientes-modal .row > div {
            margin-bottom: .8rem;
        }

        @media (max-width: 767.98px) {
            .clientes-page .Modal-padre {
                padding: .55rem;
                align-items: flex-start;
                overflow-y: auto;
            }

            .clientes-modal.modal-dialog {
                max-width: 100% !important;
            }

            .clientes-modal .modal-body {
                padding: .85rem;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="clientes-page">
        <asp:Panel ID="panelModal" Visible="false" runat="server" CssClass="Modal-padre">
            <div tabindex="-1" role="dialog" aria-labelledby="exampleModalLongTitle" aria-hidden="true">
                <div class="modal-dialog modal-lg clientes-modal" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title">
                                <asp:Label Text="Gestionar Cliente" runat="server" ID="lblTituloModal" />
                            </h5>
                            <button type="button" class="close p-0 m-0 border-0 bg-transparent" data-dismiss="modal" aria-label="Close">
                                <asp:LinkButton ID="btnCerrarModal1" runat="server" OnClick="btnCerrarModal1_Click"><i class="fas fa-times"></i></asp:LinkButton>
                            </button>
                        </div>
                        <div class="modal-body" style="max-height: 600px; overflow-y: auto">
                            <div class="row">
                                <div class="col-md-6">
                                    <label>Tipo Plan</label>
                                    <asp:DropDownList runat="server" ID="ddl_TipoPlan" CssClass="form-control mayus">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-6">
                                    <label>NIT.</label>
                                    <asp:TextBox runat="server" ID="txtNit" CssClass="form-control" TextMode="Number" />
                                </div>
                                <div class="col-md-12">
                                    <label>Nombre Comercial</label>
                                    <asp:TextBox runat="server" ID="txtNombreComersial" CssClass="form-control mayus" />
                                </div>
                                <div class="col-md-12">
                                    <label>Representante</label>
                                    <asp:TextBox runat="server" ID="txtRepresentante" CssClass="form-control mayus" />
                                </div>
                                <div class="col-md-6">
                                    <label>WhatsApp</label>
                                    <asp:TextBox runat="server" ID="txtWhatSapp" CssClass="form-control mayus" TextMode="Number" />
                                </div>
                                <div class="col-md-6">
                                    <label>Valor</label>
                                    <asp:TextBox runat="server" ID="txtValor" CssClass="form-control mayus" TextMode="Number" />
                                </div>
                                <div class="col-md-8">
                                    <label>Email.</label>
                                    <asp:TextBox runat="server" ID="txtCorreo" CssClass="form-control" TextMode="Email" />
                                </div>
                                <div class="col-md-4">
                                    <label>Sedes</label>
                                    <asp:TextBox runat="server" ID="txtSedes" CssClass="form-control mayus" TextMode="Number" />
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:LinkButton ID="btnCerrarModal2" runat="server" class="btn btn-secondary" OnClick="btnCerrarModal2_Click">Cerrar</asp:LinkButton>
                            <asp:LinkButton ID="btnGuardar" runat="server" class="btn btn-primary" OnClick="btnGuardar_Click"></asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>

        <asp:Panel ID="panelModalDB" Visible="false" runat="server" CssClass="Modal-padre">
            <div tabindex="-1" role="dialog" aria-hidden="true">
                <div class="modal-dialog modal-lg clientes-modal" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title">
                                <i class="fas fa-database"></i>&nbsp;Bases de Datos
                            </h5>
                            <button type="button" class="close p-0 m-0 border-0 bg-transparent" data-dismiss="modal" aria-label="Close">
                                <asp:LinkButton ID="btnCerrarModalDB1" runat="server" OnClick="btnCerrarModalDB_Click"><i class="fas fa-times"></i></asp:LinkButton>
                            </button>
                        </div>
                        <div class="modal-body" style="max-height: 78vh; overflow-y: auto">
                            <span class="db-client-chip"><i class="fas fa-user-tie"></i> Cliente: <asp:Label runat="server" ID="lblClienteDB" /></span>

                            <section class="db-inline-card">
                                <div class="db-inline-title">Relacionar nueva base</div>
                                <p class="db-inline-text">Busca una base disponible y asígnala al cliente seleccionado desde este mismo modal.</p>
                                <div class="db-inline-grid">
                                    <div>
                                        <label>Filtrar base</label>
                                        <input type="text" id="txtFiltroDB" class="form-control" placeholder="Escribe para buscar..." oninput="filtrarBasesCliente()" />
                                    </div>
                                    <div>
                                        <label>Lista DB</label>
                                        <asp:DropDownList runat="server" ID="ddl_db" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                    <asp:Button Text="Agregar" runat="server" ID="btn_Agregar" OnClick="btn_Agregar_Click" CssClass="btn btn-primary db-inline-btn" />
                                </div>
                            </section>

                            <section class="db-inline-table">
                                <table id="tablaDB_Clientes" class="table-cebra" style="width:100%;">
                                    <thead>
                                        <tr>
                                            <th>#</th>
                                            <th>Nombre DB</th>
                                            <th>Estado</th>
                                            <th>Opcion</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater runat="server" ID="rpDB_Cliente">
                                            <ItemTemplate>
                                                <tr>
                                                    <td><%# Container.ItemIndex +1 %></td>
                                                    <td><%# Eval("nameDataBase") %></td>
                                                    <td>
                                                        <span class='db-inline-state <%# Convert.ToInt32(Eval("estado")) == 1 ? "db-inline-state--activo" : "db-inline-state--inactivo" %>'>
                                                            <i class='fas <%# Convert.ToInt32(Eval("estado")) == 1 ? "fa-check-circle" : "fa-pause-circle" %>'></i>
                                                            <%# Convert.ToInt32(Eval("estado")) == 1 ? "Activa" : "Inactiva" %>
                                                        </span>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton runat="server" ID="btnEliminarDB" CssClass="db-inline-option" ToolTip="Eliminar relación" OnClick="btnEliminarDB_Click" CommandArgument='<%# Eval("id") %>'><i class="fas fa-trash-alt"></i></asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>
                            </section>
                        </div>
                        <div class="modal-footer">
                            <asp:LinkButton ID="btnCerrarModalDB2" runat="server" class="btn btn-secondary" OnClick="btnCerrarModalDB_Click">Cerrar</asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>

        <section class="container-fluid">
            <div class="clientes-header">
                <h1 class="clientes-header__title"><i class="fas fa-address-book"></i> Clientes</h1>
                <asp:LinkButton CssClass="btn btn-primary clientes-primary-btn" runat="server" ID="btnNuevoCliente" OnClick="btnNuevoCliente_Click"><i class="fas fa-plus"></i>&nbsp;Nuevo Cliente</asp:LinkButton>
            </div>
        </section>
        <div class="clientes-divider"></div>

        <section class="clientes-table-wrap">
            <table id="tablaClientes" class="table-cebra clientes-table">
                <thead>
                    <tr class="row-100">
                        <th class="col-1">#</th>
                        <th class="col-auto">Plan</th>
                        <th class="col-auto">NIT</th>
                        <th class="col-auto">Razon Social</th>
                        <th class="col-1">Representante</th>
                        <th class="col-1">WhatsApp</th>
                        <th class="col-1">Email</th>
                        <th class="col-1">Sedes</th>
                        <th class="col-1">Valor Plan</th>
                        <th class="col-1">Estado</th>
                        <th class="col-1">Opciones</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater runat="server" ID="rpClientes">
                        <ItemTemplate>
                            <tr class="row-100">
                                <td class="col-1"><%# Container.ItemIndex +1 %></td>
                                <td class="col-auto"><%# Eval("nombrePlan") %></td>
                                <td class="col-auto"><%# Eval("nit") %></td>
                                <td class="col-auto"><%# Eval("nombreComercial") %></td>
                                <td class="col-auto"><%# Eval("nombreRepresentate") %></td>
                                <td class="col-auto"><%# Eval("celular") %></td>
                                <td class="col-auto"><%# Eval("correo") %></td>
                                <td class="col-auto"><%# Eval("sedes") %></td>
                                <td class="col-auto"><%# $"{Eval("valorPlan"):C0}" %></td>
                                <td class="col-auto">
                                    <span class='clientes-state <%# Convert.ToInt32(Eval("estado")) == 1 ? "clientes-state--activo" : "clientes-state--inactivo" %>'>
                                        <i class='fas <%# Convert.ToInt32(Eval("estado")) == 1 ? "fa-check-circle" : "fa-pause-circle" %>'></i>
                                        <%# Convert.ToInt32(Eval("estado")) == 1 ? "Activo" : "Inactivo" %>
                                    </span>
                                    <asp:LinkButton runat="server" ID="btnEstado" CssClass="clientes-option clientes-option--toggle" ToolTip="Cambiar estado" OnClick="btnEstado_Click" CommandArgument='<%# Eval("id") %>'><i class="fas fa-sync-alt"></i></asp:LinkButton>
                                </td>
                                <td class="col-1">
                                    <asp:LinkButton runat="server" ID="btnAgregarDB" CssClass="clientes-option clientes-option--db" ToolTip="Agregar base de datos" OnClick="btnAgregarDB_Click" CommandArgument='<%# Eval("id") %>'><i class="fas fa-plus-circle"></i></asp:LinkButton>
                                    <asp:LinkButton runat="server" ID="btnEditar" CssClass="clientes-option clientes-option--edit" ToolTip="Editar cliente" OnClick="btnEditar_Click" CommandArgument='<%# Eval("id") %>'><i class="fas fa-edit"></i></asp:LinkButton>
                                    <asp:LinkButton runat="server" ID="btnEliminar" CssClass="clientes-option clientes-option--delete" ToolTip="Eliminar cliente" OnClick="btnEliminar_Click" CommandArgument='<%# Eval("id") %>'><i class="fas fa-trash-alt"></i></asp:LinkButton>
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


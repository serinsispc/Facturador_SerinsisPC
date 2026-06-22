<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="notificaiones.aspx.cs" Inherits="Facturador_SerinsisPC.notificaiones" Async="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .mensajes-page {
            display: flex;
            flex-direction: column;
            gap: .9rem;
        }

        .mensajes-page .container-fluid {
            padding-left: .2rem;
            padding-right: .2rem;
        }

        .mensajes-header {
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: 1rem;
        }

        .mensajes-header__title {
            display: flex;
            align-items: center;
            gap: .7rem;
            color: #2a22a6;
            margin: 0;
            font-size: 2rem;
            font-weight: 700;
        }

        .mensajes-header__title i {
            color: #62afe2;
            font-size: 1.7rem;
        }

        .mensajes-divider {
            height: 1px;
            background: linear-gradient(90deg, #c9e3f5 0%, rgba(201, 227, 245, 0) 100%);
        }

        .mensajes-primary-btn {
            border-radius: 12px;
            padding: .6rem 1rem;
            font-weight: 700;
            background: linear-gradient(135deg, #3c35c6 0%, #2a22a6 100%);
            border: 0;
            box-shadow: 0 12px 22px rgba(42, 34, 166, 0.16);
        }

        .mensajes-table-wrap {
            background: linear-gradient(135deg, #fbfdff 0%, #f3f9ff 100%);
            border: 1px solid #cfe2f3;
            border-radius: 16px;
            padding: .3rem;
            box-shadow: 0 10px 24px rgba(42, 34, 166, 0.05);
            overflow-x: auto;
            -webkit-overflow-scrolling: touch;
        }

        .mensajes-table {
            width: 100%;
            min-width: 760px;
        }

        .mensajes-table thead th {
            white-space: nowrap;
        }

        .mensajes-option {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            width: 38px;
            height: 38px;
            border-radius: 11px;
            text-decoration: none;
            margin-right: .25rem;
            transition: transform .15s ease, box-shadow .15s ease;
        }

        .mensajes-option:hover {
            text-decoration: none;
            transform: translateY(-1px);
            box-shadow: 0 8px 16px rgba(42, 34, 166, 0.10);
        }

        .mensajes-option--whatsapp {
            background: #ecfbf2;
            color: #1f9b53;
        }

        .mensajes-option--delete {
            background: #fff0f2;
            color: #d8344a;
        }

        .mensajes-modal .modal-dialog {
            width: 100%;
            max-width: 920px !important;
            margin: 0 auto;
        }

        .mensajes-modal .modal-content {
            border: 1px solid #cfe2f3;
            border-radius: 18px;
            box-shadow: 0 22px 46px rgba(42, 34, 166, 0.14);
            overflow: hidden;
        }

        .mensajes-modal .modal-header {
            background: linear-gradient(135deg, #2a22a6 0%, #62afe2 100%);
            color: #ffffff;
            border-bottom: 0;
            padding: .9rem 1rem;
            display: flex;
            align-items: center;
            justify-content: space-between;
        }

        .mensajes-modal .modal-title {
            font-weight: 700;
            margin: 0;
        }

        .mensajes-modal .close,
        .mensajes-modal .close a {
            color: #ffffff;
            opacity: 1;
            text-decoration: none;
        }

        .mensajes-modal .modal-body {
            background: #fbfdff;
            padding: 1rem;
        }

        .mensajes-modal .modal-footer {
            background: #f5f9ff;
            border-top: 1px solid #d8e6f4;
            padding: .8rem 1rem;
            display: flex;
            justify-content: flex-end;
            gap: .6rem;
        }

        .mensajes-modal label {
            color: #2a22a6;
            font-weight: 700;
            font-size: .88rem;
            margin-bottom: .3rem;
        }

        .mensajes-modal .form-control {
            border: 1px solid #cfe2f3;
            border-radius: 12px;
            min-height: 42px;
            color: #243a60;
            box-shadow: none;
        }

        .mensajes-modal .form-control:focus {
            border-color: #62afe2;
            box-shadow: 0 0 0 .16rem rgba(98, 175, 226, 0.18);
        }

        @media (max-width: 767.98px) {
            .mensajes-header {
                flex-direction: column;
                align-items: stretch;
            }

            .mensajes-header__title {
                font-size: 1.6rem;
            }

            .mensajes-primary-btn {
                width: 100%;
            }

            .mensajes-modal .modal-dialog {
                max-width: 100% !important;
            }

            .mensajes-modal .modal-body {
                padding: .85rem;
            }

            .mensajes-modal .modal-footer {
                flex-direction: column;
            }

            .mensajes-modal .modal-footer .btn {
                width: 100%;
            }

            .mensajes-table {
                min-width: 700px;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="mensajes-page">
        <%-- Panel modal Buscar Cliente --%>
        <asp:Panel ID="panelModalMensajes" Visible="false" runat="server" CssClass="Modal-padre mensajes-modal">
            <div tabindex="-1" role="dialog" aria-labelledby="exampleModalLongTitle" aria-hidden="true">
                <div class="modal-dialog modal-lg" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title">
                                <asp:Label Text="Buscar Cliente" runat="server" ID="lblTituloModal" />
                            </h5>
                            <button type="button" class="close p-0 m-0 border-0 bg-transparent" data-dismiss="modal" aria-label="Close">
                                <asp:LinkButton ID="btnCerrarModalBuscarCliente" runat="server" OnClick="btnCerrarModalBuscarCliente_Click"><i class="fas fa-times"></i></asp:LinkButton>
                            </button>
                        </div>
                        <div class="modal-body" style="overflow-y: auto">
                            <div class="row">
                                <div class="form-group col-md-6">
                                    <label>Titulo</label>
                                    <asp:TextBox runat="server" ID="txtTitulo" CssClass="form-control mayus" />
                                </div>
                                <div class="form-group col-md-6">
                                    <label>Variables</label>
                                    <asp:TextBox runat="server" ID="txtVariables" CssClass="form-control" TextMode="Number" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label>Mensaje</label>
                                <asp:TextBox runat="server" ID="txtMensaje" CssClass="form-control" TextMode="MultiLine" />
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:LinkButton ID="btnCerrarModalBuscarCliente2" runat="server" CssClass="btn btn-secondary" OnClick="btnCerrarModalBuscarCliente2_Click">Cerrar</asp:LinkButton>
                            <asp:LinkButton ID="btnGuardarMensaje" runat="server" CssClass="btn btn-primary" OnClick="btnGuardarMensaje_Click">Guardar Mensaje</asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>

        <section class="container-fluid">
            <div class="mensajes-header">
                <h1 class="mensajes-header__title"><i class="fas fa-comment"></i> Mensajes</h1>
                <asp:LinkButton CssClass="btn btn-primary mensajes-primary-btn" runat="server" ID="btnNuevoMensaje" OnClick="btnNuevoMensaje_Click"><i class="fas fa-plus"></i>&nbsp;Nuevo mensaje</asp:LinkButton>
            </div>
        </section>

        <div class="mensajes-divider"></div>

        <section class="mensajes-table-wrap">
            <table id="tablaMensajes" class="table-cebra mensajes-table">
                <thead>
                    <tr>
                        <th style="min-width: 15px;" class="sticky">#</th>
                        <th>Titulo</th>
                        <th>Mensaje</th>
                        <th>Opciones</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater runat="server" ID="rpMensajes">
                        <ItemTemplate>
                            <tr>
                                <td style="min-width:15px;" class="sticky"><%# Container.ItemIndex +1 %></td>
                                <td><%# Eval("nombreMensaje") %></td>
                                <td><%# Eval("mensajeText") %></td>
                                <td class="text-center">
                                    <asp:LinkButton runat="server" ID="btnWhatSapp" CssClass="mensajes-option mensajes-option--whatsapp" OnClick="btnWhatSapp_Click" CommandArgument='<%# Eval("id") %>'><i class="fab fa-whatsapp"></i></asp:LinkButton>
                                    <asp:LinkButton runat="server" ID="btnEliminarMensaje" CssClass="mensajes-option mensajes-option--delete" OnClick="btnEliminarMensaje_Click" CommandArgument='<%# Eval("id") %>'><i class="fas fa-trash-alt"></i></asp:LinkButton>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </section>
    </div>
</asp:Content>

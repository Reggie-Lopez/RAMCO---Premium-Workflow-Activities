// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSTrust.ChannelFactoryOperations
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
  [ComVisible(true)]
  public static class ChannelFactoryOperations
  {
    public static T CreateChannelActingAs<T>(this ChannelFactory<T> factory, SecurityToken actAs) => ChannelFactoryOperations.CreateChannelWithParameters<T>(factory, new FederatedClientCredentialsParameters()
    {
      ActAs = actAs
    });

    public static T CreateChannelActingAs<T>(
      this ChannelFactory<T> factory,
      EndpointAddress address,
      SecurityToken actAs)
    {
      return ChannelFactoryOperations.CreateChannelWithParameters<T>(factory, address, new FederatedClientCredentialsParameters()
      {
        ActAs = actAs
      });
    }

    public static T CreateChannelActingAs<T>(
      this ChannelFactory<T> factory,
      EndpointAddress address,
      Uri via,
      SecurityToken actAs)
    {
      return ChannelFactoryOperations.CreateChannelWithParameters<T>(factory, address, via, new FederatedClientCredentialsParameters()
      {
        ActAs = actAs
      });
    }

    public static T CreateChannelOnBehalfOf<T>(
      this ChannelFactory<T> factory,
      SecurityToken onBehalfOf)
    {
      return ChannelFactoryOperations.CreateChannelWithParameters<T>(factory, new FederatedClientCredentialsParameters()
      {
        OnBehalfOf = onBehalfOf
      });
    }

    public static T CreateChannelOnBehalfOf<T>(
      this ChannelFactory<T> factory,
      EndpointAddress address,
      SecurityToken onBehalfOf)
    {
      return ChannelFactoryOperations.CreateChannelWithParameters<T>(factory, address, new FederatedClientCredentialsParameters()
      {
        OnBehalfOf = onBehalfOf
      });
    }

    public static T CreateChannelOnBehalfOf<T>(
      this ChannelFactory<T> factory,
      EndpointAddress address,
      Uri via,
      SecurityToken onBehalfOf)
    {
      return ChannelFactoryOperations.CreateChannelWithParameters<T>(factory, address, via, new FederatedClientCredentialsParameters()
      {
        OnBehalfOf = onBehalfOf
      });
    }

    public static T CreateChannelWithIssuedToken<T>(
      this ChannelFactory<T> factory,
      SecurityToken issuedToken)
    {
      return ChannelFactoryOperations.CreateChannelWithParameters<T>(factory, new FederatedClientCredentialsParameters()
      {
        IssuedSecurityToken = issuedToken
      });
    }

    public static T CreateChannelWithIssuedToken<T>(
      this ChannelFactory<T> factory,
      EndpointAddress address,
      SecurityToken issuedToken)
    {
      return ChannelFactoryOperations.CreateChannelWithParameters<T>(factory, address, new FederatedClientCredentialsParameters()
      {
        IssuedSecurityToken = issuedToken
      });
    }

    public static T CreateChannelWithIssuedToken<T>(
      this ChannelFactory<T> factory,
      EndpointAddress address,
      Uri via,
      SecurityToken issuedToken)
    {
      return ChannelFactoryOperations.CreateChannelWithParameters<T>(factory, address, via, new FederatedClientCredentialsParameters()
      {
        IssuedSecurityToken = issuedToken
      });
    }

    internal static void VerifyChannelFactory<T>(ChannelFactory<T> channelFactory)
    {
      if (channelFactory == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (channelFactory));
      if (channelFactory.Endpoint == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID3195"));
      if (channelFactory.Endpoint.Behaviors == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID3195"));
      if (!(channelFactory.Endpoint.Behaviors.Find<ClientCredentials>() is FederatedClientCredentials))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID3196"));
    }

    public static T CreateChannelWithParameters<T>(
      ChannelFactory<T> factory,
      FederatedClientCredentialsParameters parameters)
    {
      ChannelFactoryOperations.VerifyChannelFactory<T>(factory);
      T channel = factory.CreateChannel();
      ((IChannel) (object) channel).GetProperty<ChannelParameterCollection>().Add((object) parameters);
      return channel;
    }

    public static T CreateChannelWithParameters<T>(
      ChannelFactory<T> factory,
      EndpointAddress address,
      FederatedClientCredentialsParameters parameters)
    {
      ChannelFactoryOperations.VerifyChannelFactory<T>(factory);
      T channel = factory.CreateChannel(address);
      ((IChannel) (object) channel).GetProperty<ChannelParameterCollection>().Add((object) parameters);
      return channel;
    }

    public static T CreateChannelWithParameters<T>(
      ChannelFactory<T> factory,
      EndpointAddress address,
      Uri via,
      FederatedClientCredentialsParameters parameters)
    {
      ChannelFactoryOperations.VerifyChannelFactory<T>(factory);
      T channel = factory.CreateChannel(address, via);
      ((IChannel) (object) channel).GetProperty<ChannelParameterCollection>().Add((object) parameters);
      return channel;
    }

    public static void ConfigureChannelFactory<T>(this ChannelFactory<T> channelFactory)
    {
      if (channelFactory == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (channelFactory));
      if (channelFactory.State != CommunicationState.Created && channelFactory.State != CommunicationState.Opening)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID3197"));
      ClientCredentials other = channelFactory.Endpoint.Behaviors.Find<ClientCredentials>();
      if (other == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID3212"));
      channelFactory.Endpoint.Behaviors.Remove(other.GetType());
      FederatedClientCredentials clientCredentials = new FederatedClientCredentials(other);
      channelFactory.Endpoint.Behaviors.Add((IEndpointBehavior) clientCredentials);
    }
  }
}

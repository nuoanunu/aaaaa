<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="SSMCloudService" generation="1" functional="0" release="0" Id="0c26c235-a2c7-4d40-b501-cf9809c5ce02" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="SSMCloudServiceGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="CustomerPortal:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/SSMCloudService/SSMCloudServiceGroup/LB:CustomerPortal:Endpoint1" />
          </inToChannel>
        </inPort>
        <inPort name="SiteBanHang:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/SSMCloudService/SSMCloudServiceGroup/LB:SiteBanHang:Endpoint1" />
          </inToChannel>
        </inPort>
        <inPort name="SSM:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/SSMCloudService/SSMCloudServiceGroup/LB:SSM:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="CustomerPortal:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/SSMCloudService/SSMCloudServiceGroup/MapCustomerPortal:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="CustomerPortalInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/SSMCloudService/SSMCloudServiceGroup/MapCustomerPortalInstances" />
          </maps>
        </aCS>
        <aCS name="EmailWorker:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/SSMCloudService/SSMCloudServiceGroup/MapEmailWorker:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="EmailWorkerInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/SSMCloudService/SSMCloudServiceGroup/MapEmailWorkerInstances" />
          </maps>
        </aCS>
        <aCS name="SiteBanHang:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/SSMCloudService/SSMCloudServiceGroup/MapSiteBanHang:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="SiteBanHangInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/SSMCloudService/SSMCloudServiceGroup/MapSiteBanHangInstances" />
          </maps>
        </aCS>
        <aCS name="SSM:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/SSMCloudService/SSMCloudServiceGroup/MapSSM:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="SSMInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/SSMCloudService/SSMCloudServiceGroup/MapSSMInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:CustomerPortal:Endpoint1">
          <toPorts>
            <inPortMoniker name="/SSMCloudService/SSMCloudServiceGroup/CustomerPortal/Endpoint1" />
          </toPorts>
        </lBChannel>
        <lBChannel name="LB:SiteBanHang:Endpoint1">
          <toPorts>
            <inPortMoniker name="/SSMCloudService/SSMCloudServiceGroup/SiteBanHang/Endpoint1" />
          </toPorts>
        </lBChannel>
        <lBChannel name="LB:SSM:Endpoint1">
          <toPorts>
            <inPortMoniker name="/SSMCloudService/SSMCloudServiceGroup/SSM/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapCustomerPortal:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/SSMCloudService/SSMCloudServiceGroup/CustomerPortal/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapCustomerPortalInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/SSMCloudService/SSMCloudServiceGroup/CustomerPortalInstances" />
          </setting>
        </map>
        <map name="MapEmailWorker:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/SSMCloudService/SSMCloudServiceGroup/EmailWorker/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapEmailWorkerInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/SSMCloudService/SSMCloudServiceGroup/EmailWorkerInstances" />
          </setting>
        </map>
        <map name="MapSiteBanHang:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/SSMCloudService/SSMCloudServiceGroup/SiteBanHang/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapSiteBanHangInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/SSMCloudService/SSMCloudServiceGroup/SiteBanHangInstances" />
          </setting>
        </map>
        <map name="MapSSM:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/SSMCloudService/SSMCloudServiceGroup/SSM/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapSSMInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/SSMCloudService/SSMCloudServiceGroup/SSMInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="CustomerPortal" generation="1" functional="0" release="0" software="D:\aaaaa\SSMCloudServiceOfficial\SSMCloudService\csx\Debug\roles\CustomerPortal" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="-1" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;CustomerPortal&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;CustomerPortal&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;EmailWorker&quot; /&gt;&lt;r name=&quot;SiteBanHang&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;SSM&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/SSMCloudService/SSMCloudServiceGroup/CustomerPortalInstances" />
            <sCSPolicyUpdateDomainMoniker name="/SSMCloudService/SSMCloudServiceGroup/CustomerPortalUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/SSMCloudService/SSMCloudServiceGroup/CustomerPortalFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
        <groupHascomponents>
          <role name="EmailWorker" generation="1" functional="0" release="0" software="D:\aaaaa\SSMCloudServiceOfficial\SSMCloudService\csx\Debug\roles\EmailWorker" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaWorkerHost.exe " memIndex="-1" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;EmailWorker&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;CustomerPortal&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;EmailWorker&quot; /&gt;&lt;r name=&quot;SiteBanHang&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;SSM&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/SSMCloudService/SSMCloudServiceGroup/EmailWorkerInstances" />
            <sCSPolicyUpdateDomainMoniker name="/SSMCloudService/SSMCloudServiceGroup/EmailWorkerUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/SSMCloudService/SSMCloudServiceGroup/EmailWorkerFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
        <groupHascomponents>
          <role name="SiteBanHang" generation="1" functional="0" release="0" software="D:\aaaaa\SSMCloudServiceOfficial\SSMCloudService\csx\Debug\roles\SiteBanHang" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="-1" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="8081" />
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;SiteBanHang&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;CustomerPortal&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;EmailWorker&quot; /&gt;&lt;r name=&quot;SiteBanHang&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;SSM&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/SSMCloudService/SSMCloudServiceGroup/SiteBanHangInstances" />
            <sCSPolicyUpdateDomainMoniker name="/SSMCloudService/SSMCloudServiceGroup/SiteBanHangUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/SSMCloudService/SSMCloudServiceGroup/SiteBanHangFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
        <groupHascomponents>
          <role name="SSM" generation="1" functional="0" release="0" software="D:\aaaaa\SSMCloudServiceOfficial\SSMCloudService\csx\Debug\roles\SSM" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="-1" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="8080" />
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;SSM&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;CustomerPortal&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;EmailWorker&quot; /&gt;&lt;r name=&quot;SiteBanHang&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;SSM&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/SSMCloudService/SSMCloudServiceGroup/SSMInstances" />
            <sCSPolicyUpdateDomainMoniker name="/SSMCloudService/SSMCloudServiceGroup/SSMUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/SSMCloudService/SSMCloudServiceGroup/SSMFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="CustomerPortalUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyUpdateDomain name="SiteBanHangUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyUpdateDomain name="SSMUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyUpdateDomain name="EmailWorkerUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="CustomerPortalFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyFaultDomain name="EmailWorkerFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyFaultDomain name="SiteBanHangFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyFaultDomain name="SSMFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="CustomerPortalInstances" defaultPolicy="[1,1,1]" />
        <sCSPolicyID name="EmailWorkerInstances" defaultPolicy="[1,1,1]" />
        <sCSPolicyID name="SiteBanHangInstances" defaultPolicy="[1,1,1]" />
        <sCSPolicyID name="SSMInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="84afa080-7924-4f85-b85e-7d13a81062ab" ref="Microsoft.RedDog.Contract\ServiceContract\SSMCloudServiceContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="cb8142b0-b406-4a78-ac83-157acaa720c2" ref="Microsoft.RedDog.Contract\Interface\CustomerPortal:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/SSMCloudService/SSMCloudServiceGroup/CustomerPortal:Endpoint1" />
          </inPort>
        </interfaceReference>
        <interfaceReference Id="8e7bebce-3cb7-4f6d-9561-f3919316caf6" ref="Microsoft.RedDog.Contract\Interface\SiteBanHang:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/SSMCloudService/SSMCloudServiceGroup/SiteBanHang:Endpoint1" />
          </inPort>
        </interfaceReference>
        <interfaceReference Id="2dc0bbb8-24eb-4f27-8822-6ba3e31a4e7f" ref="Microsoft.RedDog.Contract\Interface\SSM:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/SSMCloudService/SSMCloudServiceGroup/SSM:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>
// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: proto/clarifai/api/code.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Clarifai.Internal.GRPC {

  /// <summary>Holder for reflection information generated from proto/clarifai/api/code.proto</summary>
  public static partial class CodeReflection {

    #region Descriptor
    /// <summary>File descriptor for proto/clarifai/api/code.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static CodeReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Ch1wcm90by9jbGFyaWZhaS9hcGkvY29kZS5wcm90bxIMY2xhcmlmYWkuYXBp",
            "GiZwcm90by9jbGFyaWZhaS9hcGkvc3RhdHVzL3N0YXR1cy5wcm90byIuChRH",
            "ZXRTdGF0dXNDb2RlUmVxdWVzdBIWCg5zdGF0dXNfY29kZV9pZBgBIAEoCSIY",
            "ChZMaXN0U3RhdHVzQ29kZXNSZXF1ZXN0IkcKGFNpbmdsZVN0YXR1c0NvZGVS",
            "ZXNwb25zZRIrCgZzdGF0dXMYASABKAsyGy5jbGFyaWZhaS5hcGkuc3RhdHVz",
            "LlN0YXR1cyJ1ChdNdWx0aVN0YXR1c0NvZGVSZXNwb25zZRIrCgZzdGF0dXMY",
            "ASABKAsyGy5jbGFyaWZhaS5hcGkuc3RhdHVzLlN0YXR1cxItCghzdGF0dXNl",
            "cxgCIAMoCzIbLmNsYXJpZmFpLmFwaS5zdGF0dXMuU3RhdHVzQloKG2NsYXJp",
            "ZmFpMi5pbnRlcm5hbC5ncnBjLmFwaVoDYXBpogIEQ0FJUKoCFkNsYXJpZmFp",
            "LkludGVybmFsLkdSUEPCAgFfygIRQ2xhcmlmYWlcSW50ZXJuYWxiBnByb3Rv",
            "Mw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Clarifai.Internal.GRPC.Status.StatusReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Clarifai.Internal.GRPC.GetStatusCodeRequest), global::Clarifai.Internal.GRPC.GetStatusCodeRequest.Parser, new[]{ "StatusCodeId" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Clarifai.Internal.GRPC.ListStatusCodesRequest), global::Clarifai.Internal.GRPC.ListStatusCodesRequest.Parser, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Clarifai.Internal.GRPC.SingleStatusCodeResponse), global::Clarifai.Internal.GRPC.SingleStatusCodeResponse.Parser, new[]{ "Status" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Clarifai.Internal.GRPC.MultiStatusCodeResponse), global::Clarifai.Internal.GRPC.MultiStatusCodeResponse.Parser, new[]{ "Status", "Statuses" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class GetStatusCodeRequest : pb::IMessage<GetStatusCodeRequest> {
    private static readonly pb::MessageParser<GetStatusCodeRequest> _parser = new pb::MessageParser<GetStatusCodeRequest>(() => new GetStatusCodeRequest());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<GetStatusCodeRequest> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Clarifai.Internal.GRPC.CodeReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GetStatusCodeRequest() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GetStatusCodeRequest(GetStatusCodeRequest other) : this() {
      statusCodeId_ = other.statusCodeId_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GetStatusCodeRequest Clone() {
      return new GetStatusCodeRequest(this);
    }

    /// <summary>Field number for the "status_code_id" field.</summary>
    public const int StatusCodeIdFieldNumber = 1;
    private string statusCodeId_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string StatusCodeId {
      get { return statusCodeId_; }
      set {
        statusCodeId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as GetStatusCodeRequest);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(GetStatusCodeRequest other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (StatusCodeId != other.StatusCodeId) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (StatusCodeId.Length != 0) hash ^= StatusCodeId.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (StatusCodeId.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(StatusCodeId);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (StatusCodeId.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(StatusCodeId);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(GetStatusCodeRequest other) {
      if (other == null) {
        return;
      }
      if (other.StatusCodeId.Length != 0) {
        StatusCodeId = other.StatusCodeId;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            StatusCodeId = input.ReadString();
            break;
          }
        }
      }
    }

  }

  public sealed partial class ListStatusCodesRequest : pb::IMessage<ListStatusCodesRequest> {
    private static readonly pb::MessageParser<ListStatusCodesRequest> _parser = new pb::MessageParser<ListStatusCodesRequest>(() => new ListStatusCodesRequest());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<ListStatusCodesRequest> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Clarifai.Internal.GRPC.CodeReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ListStatusCodesRequest() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ListStatusCodesRequest(ListStatusCodesRequest other) : this() {
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ListStatusCodesRequest Clone() {
      return new ListStatusCodesRequest(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as ListStatusCodesRequest);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(ListStatusCodesRequest other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(ListStatusCodesRequest other) {
      if (other == null) {
        return;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
        }
      }
    }

  }

  public sealed partial class SingleStatusCodeResponse : pb::IMessage<SingleStatusCodeResponse> {
    private static readonly pb::MessageParser<SingleStatusCodeResponse> _parser = new pb::MessageParser<SingleStatusCodeResponse>(() => new SingleStatusCodeResponse());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<SingleStatusCodeResponse> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Clarifai.Internal.GRPC.CodeReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public SingleStatusCodeResponse() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public SingleStatusCodeResponse(SingleStatusCodeResponse other) : this() {
      Status = other.status_ != null ? other.Status.Clone() : null;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public SingleStatusCodeResponse Clone() {
      return new SingleStatusCodeResponse(this);
    }

    /// <summary>Field number for the "status" field.</summary>
    public const int StatusFieldNumber = 1;
    private global::Clarifai.Internal.GRPC.Status.Status status_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Clarifai.Internal.GRPC.Status.Status Status {
      get { return status_; }
      set {
        status_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as SingleStatusCodeResponse);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(SingleStatusCodeResponse other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(Status, other.Status)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (status_ != null) hash ^= Status.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (status_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(Status);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (status_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Status);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(SingleStatusCodeResponse other) {
      if (other == null) {
        return;
      }
      if (other.status_ != null) {
        if (status_ == null) {
          status_ = new global::Clarifai.Internal.GRPC.Status.Status();
        }
        Status.MergeFrom(other.Status);
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            if (status_ == null) {
              status_ = new global::Clarifai.Internal.GRPC.Status.Status();
            }
            input.ReadMessage(status_);
            break;
          }
        }
      }
    }

  }

  public sealed partial class MultiStatusCodeResponse : pb::IMessage<MultiStatusCodeResponse> {
    private static readonly pb::MessageParser<MultiStatusCodeResponse> _parser = new pb::MessageParser<MultiStatusCodeResponse>(() => new MultiStatusCodeResponse());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<MultiStatusCodeResponse> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Clarifai.Internal.GRPC.CodeReflection.Descriptor.MessageTypes[3]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MultiStatusCodeResponse() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MultiStatusCodeResponse(MultiStatusCodeResponse other) : this() {
      Status = other.status_ != null ? other.Status.Clone() : null;
      statuses_ = other.statuses_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MultiStatusCodeResponse Clone() {
      return new MultiStatusCodeResponse(this);
    }

    /// <summary>Field number for the "status" field.</summary>
    public const int StatusFieldNumber = 1;
    private global::Clarifai.Internal.GRPC.Status.Status status_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Clarifai.Internal.GRPC.Status.Status Status {
      get { return status_; }
      set {
        status_ = value;
      }
    }

    /// <summary>Field number for the "statuses" field.</summary>
    public const int StatusesFieldNumber = 2;
    private static readonly pb::FieldCodec<global::Clarifai.Internal.GRPC.Status.Status> _repeated_statuses_codec
        = pb::FieldCodec.ForMessage(18, global::Clarifai.Internal.GRPC.Status.Status.Parser);
    private readonly pbc::RepeatedField<global::Clarifai.Internal.GRPC.Status.Status> statuses_ = new pbc::RepeatedField<global::Clarifai.Internal.GRPC.Status.Status>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Clarifai.Internal.GRPC.Status.Status> Statuses {
      get { return statuses_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as MultiStatusCodeResponse);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(MultiStatusCodeResponse other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(Status, other.Status)) return false;
      if(!statuses_.Equals(other.statuses_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (status_ != null) hash ^= Status.GetHashCode();
      hash ^= statuses_.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (status_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(Status);
      }
      statuses_.WriteTo(output, _repeated_statuses_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (status_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Status);
      }
      size += statuses_.CalculateSize(_repeated_statuses_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(MultiStatusCodeResponse other) {
      if (other == null) {
        return;
      }
      if (other.status_ != null) {
        if (status_ == null) {
          status_ = new global::Clarifai.Internal.GRPC.Status.Status();
        }
        Status.MergeFrom(other.Status);
      }
      statuses_.Add(other.statuses_);
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            if (status_ == null) {
              status_ = new global::Clarifai.Internal.GRPC.Status.Status();
            }
            input.ReadMessage(status_);
            break;
          }
          case 18: {
            statuses_.AddEntriesFrom(input, _repeated_statuses_codec);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code

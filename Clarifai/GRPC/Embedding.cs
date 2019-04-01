// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: proto/clarifai/api/embedding.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Clarifai.Internal.GRPC {

  /// <summary>Holder for reflection information generated from proto/clarifai/api/embedding.proto</summary>
  public static partial class EmbeddingReflection {

    #region Descriptor
    /// <summary>File descriptor for proto/clarifai/api/embedding.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static EmbeddingReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CiJwcm90by9jbGFyaWZhaS9hcGkvZW1iZWRkaW5nLnByb3RvEgxjbGFyaWZh",
            "aS5hcGkiNwoJRW1iZWRkaW5nEhIKBnZlY3RvchgBIAMoAkICEAESFgoObnVt",
            "X2RpbWVuc2lvbnMYAiABKA1CWgobY2xhcmlmYWkyLmludGVybmFsLmdycGMu",
            "YXBpWgNhcGmiAgRDQUlQqgIWQ2xhcmlmYWkuSW50ZXJuYWwuR1JQQ8ICAV/K",
            "AhFDbGFyaWZhaVxJbnRlcm5hbGIGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Clarifai.Internal.GRPC.Embedding), global::Clarifai.Internal.GRPC.Embedding.Parser, new[]{ "Vector", "NumDimensions" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class Embedding : pb::IMessage<Embedding> {
    private static readonly pb::MessageParser<Embedding> _parser = new pb::MessageParser<Embedding>(() => new Embedding());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Embedding> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Clarifai.Internal.GRPC.EmbeddingReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Embedding() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Embedding(Embedding other) : this() {
      vector_ = other.vector_.Clone();
      numDimensions_ = other.numDimensions_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Embedding Clone() {
      return new Embedding(this);
    }

    /// <summary>Field number for the "vector" field.</summary>
    public const int VectorFieldNumber = 1;
    private static readonly pb::FieldCodec<float> _repeated_vector_codec
        = pb::FieldCodec.ForFloat(10);
    private readonly pbc::RepeatedField<float> vector_ = new pbc::RepeatedField<float>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<float> Vector {
      get { return vector_; }
    }

    /// <summary>Field number for the "num_dimensions" field.</summary>
    public const int NumDimensionsFieldNumber = 2;
    private uint numDimensions_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public uint NumDimensions {
      get { return numDimensions_; }
      set {
        numDimensions_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Embedding);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Embedding other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if(!vector_.Equals(other.vector_)) return false;
      if (NumDimensions != other.NumDimensions) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= vector_.GetHashCode();
      if (NumDimensions != 0) hash ^= NumDimensions.GetHashCode();
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
      vector_.WriteTo(output, _repeated_vector_codec);
      if (NumDimensions != 0) {
        output.WriteRawTag(16);
        output.WriteUInt32(NumDimensions);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      size += vector_.CalculateSize(_repeated_vector_codec);
      if (NumDimensions != 0) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(NumDimensions);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Embedding other) {
      if (other == null) {
        return;
      }
      vector_.Add(other.vector_);
      if (other.NumDimensions != 0) {
        NumDimensions = other.NumDimensions;
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
          case 10:
          case 13: {
            vector_.AddEntriesFrom(input, _repeated_vector_codec);
            break;
          }
          case 16: {
            NumDimensions = input.ReadUInt32();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
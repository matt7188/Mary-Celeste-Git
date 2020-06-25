using UnityEngine;

public interface IPropertyWrapper
{
    void SetFloat(int param, float value);
    void SetVector(int param, Vector4 value);
    void SetTexture(int param, Texture value);
}
public class PropertyWrapperMaterial : IPropertyWrapper
{
    public void SetFloat(int param, float value) { _target.SetFloat(param, value); }
    public void SetTexture(int param, Texture value) { _target.SetTexture(param, value); }
    public void SetVector(int param, Vector4 value) { _target.SetVector(param, value); }
    public Material _target;
}
public class PropertyWrapperMPB : IPropertyWrapper
{
    public void SetFloat(int param, float value) { _target.SetFloat(param, value); }
    public void SetTexture(int param, Texture value) { _target.SetTexture(param, value); }
    public void SetVector(int param, Vector4 value) { _target.SetVector(param, value); }
    public MaterialPropertyBlock _target;
}


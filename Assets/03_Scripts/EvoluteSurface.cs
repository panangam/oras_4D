using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EvoluteSurface {
  public float h = 0.001f;

  public delegate Vector3 Surface(float u, float v);

  Vector3 ellipsoid(float u, float v) {
    return new Vector3((float) (4*Math.Cos(u)*Math.Cos(v)),
                       (float) (2*Math.Sin(u)*Math.Cos(v)),
                       (float) (Math.Sin(v)));
  }

  Vector3 del_u(Surface X, float u, float v) {
    return (X(u, v) - X(u+h, v))/h;
  }

  Vector3 del_v(Surface X, float u, float v) {
    return (X(u, v) - X(u, v+h))/h;
  }

  Vector3 N(Surface X, float u, float v) {
    Vector3 X_u = del_u(X, u, v);
    Vector3 X_v = del_v(X, u, v);
    return Vector3.Cross(X_u, X_v).normalized;
  }

  float K(Surface X, float u, float v) {
    Vector3 N_u = (N(X, u, v) - N(X, u+h, v))/h;
    Vector3 N_v = (N(X, u, v) - N(X, u, v+h))/h;

    return Vector3.Dot(Vector3.Cross(N_u, N_v), N(X, u, v));
  }

  float H(Surface X, float u, float v) {
    Vector3 N_u = (N(X, u, v) - N(X, u+h, v))/h;
    Vector3 N_v = (N(X, u, v) - N(X, u, v+h))/h;
    Vector3 X_u = del_u(X, u, v);
    Vector3 X_v = del_v(X, u, v);

    float a = Vector3.Dot(Vector3.Cross(N_u, X_v), N(X, u, v));
    float b = Vector3.Dot(Vector3.Cross(X_u, N_v), N(X, u, v));

    return (a+b)/2;
  }

  Vector3 evolute1(Surface X, float u, float v) {
    float H_res = H(X, u, v);
    float k = K(X, u, v);
    Vector3 n = N(X, u, v);
    float r1 = -H_res + (float)Math.Sqrt(H_res*H_res-k);
    float r2 = -H_res - (float)Math.Sqrt(H_res*H_res-k);
    //float k1 = 1/r1;
    //float k2 = 1/r2;

    Vector3 e1 = X(u, v) + r1*n;

    return e1;
  }

  public Vector3 point(float u, float v) {
    return evolute1(ellipsoid, u, v);
  }
}

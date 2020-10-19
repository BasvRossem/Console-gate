using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using Visuals;

namespace Tests
{
    public class UICursorTests
    {
        public UICursor CreateUICursor()
        {
            GameObject UICursorObject = new GameObject();
            Image image = UICursorObject.AddComponent<Image>();
            UICursor cursor = UICursorObject.AddComponent<UICursor>();

            return cursor;
        }

        [UnityTest]
        public IEnumerator Show()
        {
            UICursor cursor = CreateUICursor();
            yield return null;

            cursor.Show(true);
            Assert.IsTrue(cursor.isVisible);
            cursor.Show(false);
            Assert.IsFalse(cursor.isVisible);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Blink()
        {
            UICursor cursor = CreateUICursor();
            yield return null;

            cursor.Blink(true);
            Assert.IsTrue(cursor.isBlinking);
            cursor.Blink(false);
            Assert.IsFalse(cursor.isBlinking);

            yield return null;
        }

        [UnityTest]
        public IEnumerator SetSizeGetSize()
        {
            UICursor cursor = CreateUICursor();
            yield return null;

            cursor.SetSize(new Vector2(20, 20));
            Assert.AreEqual(new Vector2(20, 20), cursor.GetSize());

            yield return null;
        }

        [UnityTest]
        public IEnumerator ResetSize()
        {
            UICursor cursor = CreateUICursor();
            yield return null;

            cursor.SetSize(new Vector2(20, 20));
            cursor.ResetSize();
            Assert.AreEqual(new Vector2(8, 18), cursor.GetSize());

            yield return null;
        }

        [UnityTest]
        public IEnumerator SetPositionCenter()
        {
            UICursor cursor = CreateUICursor();
            yield return null;

            cursor.SetPositionCenter(new Vector2(50.4f, 50.4f));
            Assert.AreEqual(new Vector3(50.4f, 50.4f, 0.0f), cursor.transform.position);

            yield return null;
        }

        [UnityTest]
        public IEnumerator SetPositionTopLeft()
        {
            UICursor cursor = CreateUICursor();
            yield return null;

            cursor.SetPositionTopLeft(new Vector2(50.4f, 50.4f));
            Assert.AreEqual(new Vector3(50.4f, 50.4f, 0.0f) + new Vector3(4, -9, 0), cursor.transform.position);

            yield return null;
        }
    }
}
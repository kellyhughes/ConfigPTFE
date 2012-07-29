using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace ConfigPTFE {
	public static class XExtensions {
		/// <summary>
		/// Get the XPath to a given XElement, including the namespace.
		/// (e.g. "/a:people/b:person/c:name/d:last").
		/// </summary>
		public static string GetXPath(this XElement element) {
			if (element == null) {
				throw new ArgumentNullException("element");
			}

			Func<XElement, string> relativeXPath = e => {
			                                       	var currentNamespace = e.Name.Namespace;

			                                       	string name;
			                                       	if (currentNamespace == null || currentNamespace == string.Empty) {
			                                       		name = e.Name.LocalName;
			                                       	}
			                                       	else {
			                                       		string namespacePrefix = e.GetPrefixOfNamespace(currentNamespace);
			                                       		name = namespacePrefix + ":" + e.Name.LocalName;
			                                       	}

			                                       	return string.Format("/{0}", name);
			                                       };

			var ancestors = from e in element.Ancestors()
			                select relativeXPath(e);

			return string.Concat(ancestors.Reverse().ToArray()) +
			       relativeXPath(element);
		}

		/// <summary>
		/// Get the absolute XPath to a given XElement, including the namespace.
		/// (e.g. "/a:people/b:person[6]/c:name[1]/d:last[1]").
		/// </summary>
		public static string GetAbsoluteXPath(this XElement element) {
			if (element == null) {
				throw new ArgumentNullException("element");
			}

			Func<XElement, string> relativeXPath = e => {
			                                       	int index = e.IndexPosition();

			                                       	var currentNamespace = e.Name.Namespace;

			                                       	string name;
			                                       	if (currentNamespace == null || currentNamespace == string.Empty) {
			                                       		name = e.Name.LocalName;
			                                       	}
			                                       	else {
			                                       		string namespacePrefix = e.GetPrefixOfNamespace(currentNamespace);
			                                       		name = namespacePrefix + ":" + e.Name.LocalName;
			                                       	}

			                                       	// If the element is the root, no index is required
			                                       	return (index == -1)
			                                       	       	? "/" + name
			                                       	       	: string.Format(
			                                       	       		"/{0}[{1}]",
			                                       	       		name,
			                                       	       		index.ToString()
			                                       	       	  	);
			                                       };

			var ancestors = from e in element.Ancestors()
			                select relativeXPath(e);

			return string.Concat(ancestors.Reverse().ToArray()) +
			       relativeXPath(element);
		}

		/// <summary>
		/// Get the index of the given XElement relative to its
		/// siblings with identical names. If the given element is
		/// the root, -1 is returned.
		/// </summary>
		/// <param name="element">
		/// The element to get the index of.
		/// </param>
		public static int IndexPosition(this XElement element) {
			if (element == null) {
				throw new ArgumentNullException("element");
			}

			if (element.Parent == null) {
				return -1;
			}

			int i = 1; // Indexes for nodes start at 1, not 0

			foreach (var sibling in element.Parent.Elements(element.Name)) {
				if (sibling == element) {
					return i;
				}

				i++;
			}

			throw new InvalidOperationException
				("element has been removed from its parent.");
		}

		/// <summary>
		/// Transform an XmlNode into an XElement
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		public static XElement GetXElement(this XmlNode node) {
			XDocument xDoc = new XDocument();
			using (XmlWriter xmlWriter = xDoc.CreateWriter())
				node.WriteTo(xmlWriter);
			return xDoc.Root;
		}

		/// <summary>
		/// Transform an XElement into an XmlNode
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public static XmlNode GetXmlNode(this XElement element) {
			XmlDocument xmlDoc = new XmlDocument();
			using (XmlReader xmlReader = element.CreateReader()) {
				xmlDoc.Load(xmlReader);
			}
			return xmlDoc;
		}

		public static string InnerXml(this XElement element) {
			string retval = string.Empty;

			using (XmlReader xmlReader = element.CreateReader()) {
				xmlReader.MoveToContent();
				retval = xmlReader.ReadInnerXml();

				// ReadInnerXml() adds a namespace attribute which is not valid for the returned string when we parse it later.
				// We'll add the namespace attribute to the parent we will inject this InnerXml into.
				retval = retval.Replace("xmlns:xdt=\"http://schemas.microsoft.com/XML-Document-Transform\" ", string.Empty);
			}

			return retval;
		}
	}
}
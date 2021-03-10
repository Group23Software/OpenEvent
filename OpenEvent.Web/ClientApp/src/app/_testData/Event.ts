import {EventHostModel} from "../_models/Event";
import {SocialMedia} from "../_models/SocialMedia";
import {Address} from "../_models/Address";

export const FakeEventHostModel: EventHostModel = {
  Promos: [],
  Transactions: [],
  Address: {AddressLine1: "", AddressLine2: "", City: "", CountryCode: "", CountryName: "", PostalCode: ""},
  Categories: [
    {Name: "Music"},
    {Name: "Performing"}
  ],
  Description: "this is a host event model",
  EndLocal: new Date(0),
  EndUTC: new Date(0),
  Id: "1",
  Images: [
    {Label: "test image", Source: "IMAGE"}
  ],
  IsOnline: false,
  Name: "tests host event",
  Price: 1010,
  SocialLinks: [
    {SocialMedia: SocialMedia.Site, Link: "http://www.example.com"},
    {SocialMedia: SocialMedia.Instagram, Link: "http://www.example.com"},
    {SocialMedia: SocialMedia.Twitter, Link: "http://www.example.com"},
    {SocialMedia: SocialMedia.Facebook, Link: "http://www.example.com"},
    {SocialMedia: SocialMedia.Reddit, Link: "http://www.example.com"}
  ],
  StartLocal: new Date(0),
  StartUTC: new Date(0),
  Thumbnail: {Label: "thumbnail", Source: "THUMBNAIL"},
  Tickets: [],
  TicketsLeft: 0
}

export const FakeAddress: Address = {
  AddressLine1: "Goldsmiths, University of London",
  AddressLine2: "",
  City: "London",
  CountryCode: "GB",
  CountryName: "United Kingdom",
  PostalCode: "SE14 6NW",
  Lat: 1,
  Lon: 1
}
